using Blish_HUD;
using Blish_HUD.Controls;
using Nekres.Musician.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nekres.Musician.UI.Models;
using SQLite;

namespace Nekres.Musician.UI
{
    internal class MusicSheetFactory : IDisposable
    {
        public event EventHandler<ValueEventArgs<MusicSheetModel>> OnSheetUpdated;
        public event EventHandler<ValueEventArgs<Guid>> OnSheetRemoved;

        private FileSystemWatcher _xmlWatcher;

        public string CacheDir { get; private set; }

        private SQLiteAsyncConnection _db;

        public MusicSheetFactory(string cacheDir)
        {
            this.CacheDir = cacheDir;

            _xmlWatcher = new FileSystemWatcher(cacheDir)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.xml",
                EnableRaisingEvents = true
            };
            _xmlWatcher.Changed += OnXmlCreated;
        }

        private async void OnXmlCreated(object sender, FileSystemEventArgs e) => await ConvertXml(e.FullPath);

        private async Task ConvertXml(string filePath)
        {
            var musicSheet = MusicSheet.FromXml(filePath);
            if (musicSheet == null) return;
            await FileUtil.DeleteAsync(filePath);
            await AddOrUpdate(musicSheet);
        }

        public async Task LoadAsync()
        {
            await LoadIndex();

            var initialFiles = Directory.EnumerateFiles(this.CacheDir).Where(s => Path.GetExtension(s).Equals(".xml"));
            foreach (var filePath in initialFiles) await ConvertXml(filePath);
        }

        private async Task LoadIndex()
        {
            var filePath = Path.Combine(this.CacheDir, "index.sqlite");
            _db = new SQLiteAsyncConnection(filePath);
            await _db.CreateTableAsync<MusicSheetModel>();
        }

        public async Task AddOrUpdate(MusicSheet musicSheet)
        {
            
            var sheet = await _db.Table<MusicSheetModel>().FirstOrDefaultAsync(x => x.Id.Equals(musicSheet.Id));

            if (sheet == null)
            {
                await _db.InsertAsync(musicSheet.ToModel());
            }
            else
            {
                await _db.UpdateAsync(musicSheet.ToModel());
            }

        }

        public async Task Delete(Guid key)
        {
            await _db.Table<MusicSheetModel>().DeleteAsync(x => x.Id.Equals(key));
        }

        public void Dispose()
        {
            _xmlWatcher.Created -= OnXmlCreated;
            _xmlWatcher.Changed -= OnXmlCreated;
            _xmlWatcher.Dispose();
            _db.CloseAsync();
        }

        public async Task<MusicSheetModel> GetById(Guid id)
        {
            return await _db.Table<MusicSheetModel>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<MusicSheetModel>> GetAll()
        {
            return await _db.Table<MusicSheetModel>().ToListAsync();
        }
    }
}
