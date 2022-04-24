﻿using Blish_HUD;
using Blish_HUD.Controls;
using Nekres.Musician.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nekres.Musician.UI
{
    internal class MusicSheetFactory : IDisposable
    {
        public event EventHandler<ValueEventArgs<MusicSheetBase>> OnSheetUpdated;
        public event EventHandler<ValueEventArgs<Guid>> OnSheetRemoved;

        private FileSystemWatcher _xmlWatcher;

        private IList<MusicSheet> _fetched;

        private Dictionary<Guid, MusicSheetBase> _index;
        public Dictionary<Guid, MusicSheetBase> Index => new(_index);

        public string CacheDir { get; private set; }

        private readonly string _indexFileName;
        public MusicSheetFactory(string cacheDir)
        {
            _indexFileName = "index.json";
            _fetched = new List<MusicSheet>();
            _index = new Dictionary<Guid, MusicSheetBase>();
            this.CacheDir = cacheDir;

            _xmlWatcher = new FileSystemWatcher(cacheDir)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.xml",
                EnableRaisingEvents = true
            };
            _xmlWatcher.Created += OnXmlCreated;
            _xmlWatcher.Changed += OnXmlCreated;
        }

        private async void OnXmlCreated(object sender, FileSystemEventArgs e) => await ConvertXml(e.FullPath);

        private async Task ConvertXml(string filePath)
        {
            var musicSheet = MusicSheet.FromXml(filePath);
            if (musicSheet == null) return;
            var json = JsonConvert.SerializeObject(musicSheet, Formatting.Indented);
            var fileName = Path.Combine(Path.GetDirectoryName(filePath), $"{musicSheet.Id}.json");
            await FileUtil.WriteAllTextAsync(fileName, json);
            await FileUtil.DeleteAsync(filePath, false);

            AddOrUpdate(musicSheet);
        }

        public async Task LoadAsync()
        {
            await LoadIndex();

            var initialFiles = Directory.EnumerateFiles(this.CacheDir).Where(s => Path.GetExtension(s).Equals(".xml"));
            foreach (var filePath in initialFiles) await ConvertXml(filePath);
        }

        private async Task LoadIndex()
        {
            var filePath = Path.Combine(this.CacheDir, _indexFileName);
            if (!File.Exists(filePath))
            {
                this.SaveIndex();
                return;
            }

            try
            {
                using var str = new StreamReader(Path.Combine(this.CacheDir, _indexFileName));
                var content = await str.ReadToEndAsync();
                _index = JsonConvert.DeserializeObject<Dictionary<Guid, MusicSheetBase>>(content);

                if (_index == null)
                    throw new JsonException("No data after deserialization. Possibly corrupted Json.");
            }
            catch (Exception ex) when (ex is IOException or InvalidOperationException or JsonException)
            {
                ScreenNotification.ShowNotification("Resources.There_was_an_error_loading_your_library_", ScreenNotification.NotificationType.Error);
                MusicianModule.Logger.Error(ex, ex.Message);
            }
        }

        private void AddOrUpdate(MusicSheet musicSheet)
        {
            if (_index.TryGetValue(musicSheet.Id, out var sheet))
            {
                sheet.Artist = sheet.Artist;
                sheet.Title = sheet.Title;
                sheet.User = sheet.User;
                return;
            }

            var info = musicSheet.GetBaseInfo();
            _index.Add(musicSheet.Id, info);

            OnSheetUpdated?.Invoke(this, new ValueEventArgs<MusicSheetBase>(info));
            this.SaveIndex();
        }

        private void RemoveFromIndex(Guid key)
        {
            if (_index.ContainsKey(key))
                _index.Remove(key);

            OnSheetRemoved?.Invoke(this, new ValueEventArgs<Guid>(key));
            this.SaveIndex();
        }

        public void Dispose()
        {
            _xmlWatcher.Created -= OnXmlCreated;
            _xmlWatcher.Changed -= OnXmlCreated;
            _xmlWatcher.Dispose();
        }

        public async Task<MusicSheet> FromCache(Guid id)
        {
            var filePath = Path.Combine(this.CacheDir, $"{id}.json");
            var musicSheetModel = _fetched.FirstOrDefault(x => x.Id.Equals(id));
            if (musicSheetModel != null) return musicSheetModel;
            try
            {
                using var str = new StreamReader(filePath);
                var content = await str.ReadToEndAsync();
                musicSheetModel = JsonConvert.DeserializeObject<MusicSheet>(content);
                if (musicSheetModel == null)
                    throw new JsonException("No data after deserialization. Possibly corrupted Json.");
            }
            catch (Exception ex) when (ex is IOException or InvalidOperationException or JsonException)
            {
                ScreenNotification.ShowNotification("Resources.there_was_an_error_loading_your_music_sheet_", ScreenNotification.NotificationType.Error);
                MusicianModule.Logger.Error(ex, ex.Message);
                return null;
            }
            _fetched.Add(musicSheetModel);
            return musicSheetModel;
        }

        private async void SaveIndex()
        {
            var fileContents = Encoding.Default.GetBytes(JsonConvert.SerializeObject(_index, Formatting.Indented));

            var fileName = Path.Combine(this.CacheDir, _indexFileName);

            try
            {
                using var sourceStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize: 4096, useAsync: true);
                await sourceStream.WriteAsync(fileContents, 0, fileContents.Length);
            }
            catch (UnauthorizedAccessException)
            {
                ScreenNotification.ShowNotification("Resources.Saving_index_file_failed_ + ' ' + Resources.Access_denied_", ScreenNotification.NotificationType.Error);
            }
            catch (IOException ex)
            {
                MusicianModule.Logger.Error(ex, ex.Message);
            }
        }

        public void Delete(MusicSheet model)
        {
            var path = this.GetFilePath(model.Id);
            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
                ScreenNotification.ShowNotification(string.Format("Resources.Deletion_of__0__failed_", $"\u201c{model.Title}\u201d") + ' ' + "Resources.Access_denied_", ScreenNotification.NotificationType.Error);
                return;
            }
            catch (IOException ex)
            {
                MusicianModule.Logger.Warn(ex, ex.Message);
            }

            this.RemoveFromIndex(model.Id);
        }

        private string GetFilePath(Guid id)
        {
            return Path.Combine(this.CacheDir, $"{id}.json");
        }
    }
}
