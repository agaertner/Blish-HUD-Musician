using System;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
    public class FlutePreview : InstrumentBase
    {
        private readonly FluteSoundRepository _soundRepository;

        public FlutePreview(FluteSoundRepository soundRepo)
        {
            this.CurrentOctave = Octave.Low;
            _soundRepository = soundRepo;
        }

        protected override NoteBase ConvertNote(RealNote note) => FluteNote.From(note);

        protected override NoteBase OptimizeNote(NoteBase note) => note;

        protected override void IncreaseOctave()
        {
            switch (this.CurrentOctave)
            {
                case Octave.Low:
                    this.CurrentOctave = Octave.High;
                    break;
                case Octave.High:
                    break;
                default: break;
            }
        }

        protected override void DecreaseOctave()
        {
            switch (this.CurrentOctave)
            {
                case Octave.Low:
                    break;
                case Octave.High:
                    this.CurrentOctave = Octave.Low;
                    break;
                default: break;
            }
        }

        protected override void PressKey(GuildWarsControls key)
        {
            switch (key)
            {
                case GuildWarsControls.WeaponSkill1:
                case GuildWarsControls.WeaponSkill2:
                case GuildWarsControls.WeaponSkill3:
                case GuildWarsControls.WeaponSkill4:
                case GuildWarsControls.WeaponSkill5:
                case GuildWarsControls.HealingSkill:
                case GuildWarsControls.UtilitySkill1:
                case GuildWarsControls.UtilitySkill2:
                    MusicianModule.ModuleInstance.MusicPlayer.PlaySound(_soundRepository.Get(key, this.CurrentOctave), true);
                    break;
                case GuildWarsControls.UtilitySkill3:
                    if (this.CurrentOctave == Octave.Low)
                    {
                        IncreaseOctave();
                    }
                    else
                    {
                        DecreaseOctave();
                    }
                    break;
                case GuildWarsControls.EliteSkill:
                    MusicianModule.ModuleInstance.MusicPlayer.StopSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Dispose() {
        }
    }
}