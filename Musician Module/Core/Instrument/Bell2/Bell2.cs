using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;
using System;
using System.Threading;

namespace Nekres.Musician.Core.Instrument.Bell2
{
    public class Bell2 : BaseInstrument
    {
        public Bell2()
        {
            this.CurrentOctave = Octave.Low;
        }

        protected override BaseNote OptimizeNote(BaseNote note)
        {
            if (note.Equals(new Bell2Note(GuildWarsControls.WeaponSkill1, Octave.High)) && CurrentOctave == Octave.Low)
            {
                note = new Bell2Note(GuildWarsControls.UtilitySkill2, Octave.Low);
            }
            else if (note.Equals(new Bell2Note(GuildWarsControls.UtilitySkill2, Octave.Low)) && CurrentOctave == Octave.High)
            {
                note = new Bell2Note(GuildWarsControls.WeaponSkill1, Octave.High);
            }
            return note;
        }

        protected override BaseNote ConvertNote(RealNote note) => Bell2Note.From(note);

        protected override void IncreaseOctave()
        {
            switch (CurrentOctave)
            {
                case Octave.Low:
                    CurrentOctave = Octave.High;
                    break;
                case Octave.High:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PressKey(GuildWarsControls.EliteSkill);

            Thread.Sleep(OctaveTimeout);
        }

        protected override void DecreaseOctave()
        {
            switch (CurrentOctave)
            {
                case Octave.Low:
                    break;
                case Octave.High:
                    CurrentOctave = Octave.Low;
                    break;
                default: break;
            }

            PressKey(GuildWarsControls.UtilitySkill3);

            Thread.Sleep(OctaveTimeout);
        }

        public override void Dispose() {
        }
    }
}