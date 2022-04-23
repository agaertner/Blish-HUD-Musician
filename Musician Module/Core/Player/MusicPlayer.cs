using Microsoft.Xna.Framework.Audio;
using Nekres.Musician_Module.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nekres.Musician;
using Nekres.Musician.Core.Instrument;
using Nekres.Musician.Core.Instrument.Bass;
using Nekres.Musician.Core.Instrument.Bell;
using Nekres.Musician.Core.Instrument.Bell2;
using Nekres.Musician.Core.Instrument.Flute;
using Nekres.Musician.Core.Instrument.Harp;
using Nekres.Musician.Core.Instrument.Lute;
using Nekres.Musician.Core.Player.Algorithms;
using Nekres.Musician_Module.Controls.Instrument;

namespace Nekres.Musician_Module.Player
{
    public class MusicPlayer : IDisposable
    {
        public Thread Worker { get; private set; }
        public IPlayAlgorithm Algorithm { get; private set; }

        private SoundEffectInstance _activeSfx;

        private float _audioVolume => MusicianModule.ModuleInstance.audioVolume.Value / 1000;
        public void PlaySound(SoundEffectInstance sfx, bool loops = false)
        {
            if (loops)
            {
                StopSound();
                sfx.IsLooped = true;
            }
            _activeSfx = sfx;
            sfx.Volume = _audioVolume;
            sfx.Play();
        }

        public void StopSound()
        {
            _activeSfx?.Stop();
        }

        public MusicPlayer(MusicSheet musicSheet, BaseInstrument instrument, IPlayAlgorithm algorithm)
        {
            Algorithm = algorithm;
            Worker = new Thread(() => algorithm.Play(instrument, musicSheet.MetronomeMark, musicSheet.Melody.ToArray()));
        }

        public void PlayMusicSheet(Musician.Core.Models.MusicSheet musicSheet, InstrumentMode mode)
        {
            var algorithm = musicSheet.Algorithm == Musician.Core.Models.Algorithm.FavorNotes ? new FavorNotesAlgorithm() : (IPlayAlgorithm)new FavorChordsAlgorithm();

            switch (musicSheet.Instrument)
            {
                case Models.Instrument.Bass:
                    return new MusicPlayer(musicSheet, new Bass(), algorithm);
                case Models.Instrument.Bell:
                    return new MusicPlayer(musicSheet, new Bell(), algorithm);
                case Models.Instrument.Bell2:
                    return new MusicPlayer(musicSheet, new Bell2(), algorithm);
                case Models.Instrument.Flute:
                    return new MusicPlayer(musicSheet, new Flute(), algorithm);
                case Models.Instrument.Harp:
                    return new MusicPlayer(musicSheet, new Harp(), algorithm);
                case Models.Instrument.Horn:
                    return new MusicPlayer(musicSheet, new Horn(), algorithm);
                case Models.Instrument.Lute:
                    return new MusicPlayer(musicSheet, new Lute(), algorithm);
            }
        }

        public async Task Initialize()
        {

        }

        public void Dispose()
        {
            StopSound();
            Algorithm.Dispose();
        }
    }
}