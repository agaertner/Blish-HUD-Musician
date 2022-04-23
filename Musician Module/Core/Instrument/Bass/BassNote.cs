using System.Collections.Generic;
using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument.Bass {
    public class BassNote : BaseNote
    {
        private static readonly Dictionary<string, BassNote> Map = new()
        {
            {$"{Note.C}{Octave.Middle}", new BassNote(GuildWarsControls.WeaponSkill1, Octave.Low)},
            {$"{Note.D}{Octave.Middle}", new BassNote(GuildWarsControls.WeaponSkill2, Octave.Low)},
            {$"{Note.E}{Octave.Middle}", new BassNote(GuildWarsControls.WeaponSkill3, Octave.Low)},
            {$"{Note.F}{Octave.Middle}", new BassNote(GuildWarsControls.WeaponSkill4, Octave.Low)},
            {$"{Note.G}{Octave.Middle}", new BassNote(GuildWarsControls.WeaponSkill5, Octave.Low)},
            {$"{Note.A}{Octave.Middle}", new BassNote(GuildWarsControls.HealingSkill, Octave.Low)},
            {$"{Note.B}{Octave.Middle}", new BassNote(GuildWarsControls.UtilitySkill1, Octave.Low)},
            {$"{Note.C}{Octave.Low}", new BassNote(GuildWarsControls.WeaponSkill1, Octave.High)},
            {$"{Note.D}{Octave.Low}", new BassNote(GuildWarsControls.WeaponSkill2, Octave.High)},
            {$"{Note.E}{Octave.Low}", new BassNote(GuildWarsControls.WeaponSkill3, Octave.High)},
            {$"{Note.F}{Octave.Low}", new BassNote(GuildWarsControls.WeaponSkill4, Octave.High)},
            {$"{Note.G}{Octave.Low}", new BassNote(GuildWarsControls.WeaponSkill5, Octave.High)},
            {$"{Note.A}{Octave.Low}", new BassNote(GuildWarsControls.HealingSkill, Octave.High)},
            {$"{Note.B}{Octave.Low}", new BassNote(GuildWarsControls.UtilitySkill1, Octave.High)},
            {$"{Note.C}{Octave.Lowest}", new BassNote(GuildWarsControls.UtilitySkill2, Octave.High)}
        };

        public BassNote(GuildWarsControls key, Octave octave) : base(key, octave)
        {
            /* NOOP */
        }

        public static BassNote From(RealNote note)
        {
            return Map[$"{note.Note}{note.Octave}"];
        }

        public override bool Equals(object obj)
        {
            return Equals((BassNote) obj);
        }

        protected bool Equals(BassNote other)
        {
            return this.Key == other.Key && Octave == other.Octave;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key * 397) ^ (int) Octave;
            }
        }
    }
}