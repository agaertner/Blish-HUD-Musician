﻿using Blish_HUD;
using Microsoft.Xna.Framework;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;
using System.Diagnostics;

namespace Nekres.Musician.Core.Player.Algorithms
{
    public abstract class PlayAlgorithmBase
    {
        protected bool _abort;

        protected readonly Stopwatch _stopwatch;

        public readonly InstrumentBase Instrument;

        public readonly Vector3 CharacterPosition;

        protected PlayAlgorithmBase(InstrumentBase instrument)
        {
            Instrument = instrument;
            _stopwatch = new Stopwatch();
            CharacterPosition = GameService.Gw2Mumble.PlayerCharacter.Position;
        }

        public abstract void Play(Metronome metronomeMark, ChordOffset[] melody);

        public virtual void Dispose()
        {
            _abort = true;
            _stopwatch.Stop();
            MusicianModule.ModuleInstance.MusicPlayer?.Stop();
        }

        public void Terminate()
        {
            if (_abort) return;
            this.Dispose();
        }

        protected bool CharacterMoved()
        {
            return MusicianModule.ModuleInstance.stopWhenMoving.Value && !CharacterPosition.Equals(GameService.Gw2Mumble.PlayerCharacter.Position);
        }

        protected bool CanContinue()
        {
            return GameService.GameIntegration.Gw2Instance.Gw2HasFocus
                   && GameService.Gw2Mumble.IsAvailable
                   && !GameService.Gw2Mumble.UI.IsTextInputFocused 
                   && (!CharacterMoved() || Instrument.Walkable);
        }
    }
}