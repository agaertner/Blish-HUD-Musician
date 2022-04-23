using System;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
    public class HornPreview : InstrumentBase
    {
        private readonly HornSoundRepository _soundRepository;

        public HornPreview(HornSoundRepository soundRepo)
        {
            this.CurrentOctave = Octave.Middle;
            _soundRepository = soundRepo;
        }

        protected override NoteBase ConvertNote(RealNote note) => HornNote.From(note);

        protected override NoteBase OptimizeNote(NoteBase note)
        {
            throw new NotImplementedException();
        }

        protected override void IncreaseOctave()
        {
            switch (this.CurrentOctave)
            {
                case Octave.Low:
                    this.CurrentOctave = Octave.Middle;
                    break;
                case Octave.Middle:
                    this.CurrentOctave = Octave.High;
                    break;
                case Octave.High:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void DecreaseOctave()
        {
            switch (this.CurrentOctave)
            {
                case Octave.Low:
                    break;
                case Octave.Middle:
                    this.CurrentOctave = Octave.Low;
                    break;
                case Octave.High:
                    this.CurrentOctave = Octave.Middle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                    DecreaseOctave();
                    break;
                case GuildWarsControls.EliteSkill:
                    IncreaseOctave();
                    break;
                default: break;
            }
        }

        public override void Dispose() {
        }
    }
}