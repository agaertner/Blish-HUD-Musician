﻿using Blish_HUD;
using LiteDB;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LiteDB.Async;

namespace Nekres.Musician.UI
{
    internal class MusicSheetService : IDisposable
    {
        public event EventHandler<ValueEventArgs<MusicSheetModel>> OnSheetUpdated;

        public string CacheDir { get; private set; }

        private LiteDatabaseAsync _db;
        private ILiteCollectionAsync<MusicSheetModel> _ctx;

        private SoundEffect[] _deleteSfx;
        public SoundEffect DeleteSfx => _deleteSfx[RandomUtil.GetRandom(0, 1)];

        public MusicSheetService(string cacheDir)
        {
            _deleteSfx = new []
            {
                MusicianModule.ModuleInstance.ContentsManager.GetSound(@"audio\crumbling-paper-1.wav"),
                MusicianModule.ModuleInstance.ContentsManager.GetSound(@"audio\crumbling-paper-2.wav")
            }; 
            this.CacheDir = cacheDir;
        }

        public async Task LoadAsync()
        {
            await LoadDatabase();
        }

        private async Task LoadDatabase()
        {
            var filePath = Path.Combine(this.CacheDir, "data.db");
            _db = new LiteDatabaseAsync(filePath);
            _ctx = _db.GetCollection<MusicSheetModel>("music_sheets");
            BsonMapper.Global.Entity<MusicSheetModel>()
                .Id(x => x.InternalId)
                .Field(x => x.Id, "id")
                .Field(x => x.User, "user")
                .Field(x => x.Title, "title")
                .Field(x => x.Artist, "artist")
                .Field(x => x.Instrument, "instrument")
                .Field(x => x.Algorithm, "algorithm")
                .Field(x => x.Tempo, "tempo")
                .Field(x => x.Melody, "melody");
        }

        public async Task AddOrUpdate(MusicSheet musicSheet, bool silent = false)
        {
            var model = musicSheet.ToModel();
            await _ctx.UpsertAsync(model);
            OnSheetUpdated?.Invoke(this, new ValueEventArgs<MusicSheetModel>(model));

            if (silent) return;
            GameService.Content.PlaySoundEffectByName("color-change");
        }

        public async Task Delete(Guid key)
        {
            DeleteSfx.Play(GameService.GameIntegration.Audio.Volume, 0, 0);
            await _ctx.DeleteManyAsync(x => x.Id.Equals(key));
        }

        public void Dispose()
        {
            foreach (var sfx in _deleteSfx) sfx?.Dispose();
            _db?.Dispose();
        }

        public async Task<MusicSheetModel> GetById(Guid id)
        {
           return await _ctx.FindOneAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<MusicSheetModel>> GetAll()
        {
            return await _ctx.FindAllAsync();
        }
    }
}
