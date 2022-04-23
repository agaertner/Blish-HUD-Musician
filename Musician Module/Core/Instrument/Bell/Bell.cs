using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;
using System;
using System.Threading;

namespace Nekres.Musician.Core.Instrument.Bell
{
    public class Bell : BaseInstrument
    {
        public Bell()
        {
            this.CurrentOctave = Octave.Middle;
        }

        protected override BaseNote ConvertNote(RealNote note) => BellNote.From(note);

        protected override BaseNote OptimizeNote(BaseNote note)
        {
            if (note.Equals(new BellNote(GuildWarsControls.WeaponSkill1, Octave.High)) && CurrentOctave == Octave.Middle)
            {
                note = new BellNote(GuildWarsControls.UtilitySkill2, Octave.Middle);
            }
            else if (note.Equals(new BellNote(GuildWarsControls.UtilitySkill2, Octave.Middle)) && CurrentOctave == Octave.High)
            {
                note = new BellNote(GuildWarsControls.WeaponSkill1, Octave.High);
            }
            else if (note.Equals(new BellNote(GuildWarsControls.WeaponSkill1, Octave.Middle)) && CurrentOctave == Octave.Low)
            {
                note = new BellNote(GuildWarsControls.UtilitySkill2, Octave.Low);
            }
            else if (note.Equals(new BellNote(GuildWarsControls.UtilitySkill2, Octave.Low)) && CurrentOctave == Octave.Middle)
            {
                note = new BellNote(GuildWarsControls.WeaponSkill1, Octave.Middle);
            }
            return note;
        }

        protected override void IncreaseOctave()
        {
            switch (CurrentOctave)
            {
                case Octave.Low:
                    CurrentOctave = Octave.Middle;
                    break;
                case Octave.Middle:
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
                case Octave.Middle:
                    CurrentOctave = Octave.Low;
                    break;
                case Octave.High:
                    CurrentOctave = Octave.Middle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PressKey(GuildWarsControls.UtilitySkill3);

            Thread.Sleep(OctaveTimeout);
        }

        public override void Dispose() {
        }
    }
}