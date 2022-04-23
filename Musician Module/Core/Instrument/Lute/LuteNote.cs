using Nekres.Musician.Core.Domain;
using System.Collections.Generic;

namespace Nekres.Musician.Core.Instrument.Lute
{
    public class LuteNote
    {
        public enum Keys
        {
            None,
            Note1,
            Note2,
            Note3,
            Note4,
            Note5,
            Note6,
            Note7,
            Note8
        }

        public enum Octaves
        {
            None,
            Low,
            Middle,
            High
        }

        private static readonly Dictionary<string, LuteNote> Map = new Dictionary<string, LuteNote>
        {
            {$"{Domain.Note.C}{Domain.Octave.Lowest}", new LuteNote(Keys.Note1, Octaves.Low)},
            {$"{Domain.Note.D}{Domain.Octave.Lowest}", new LuteNote(Keys.Note2, Octaves.Low)},
            {$"{Domain.Note.E}{Domain.Octave.Lowest}", new LuteNote(Keys.Note3, Octaves.Low)},
            {$"{Domain.Note.F}{Domain.Octave.Lowest}", new LuteNote(Keys.Note4, Octaves.Low)},
            {$"{Domain.Note.G}{Domain.Octave.Lowest}", new LuteNote(Keys.Note5, Octaves.Low)},
            {$"{Domain.Note.A}{Domain.Octave.Lowest}", new LuteNote(Keys.Note6, Octaves.Low)},
            {$"{Domain.Note.B}{Domain.Octave.Lowest}", new LuteNote(Keys.Note7, Octaves.Low)},
            {$"{Domain.Note.C}{Domain.Octave.Low}", new LuteNote(Keys.Note1, Octaves.Middle)},
            {$"{Domain.Note.D}{Domain.Octave.Low}", new LuteNote(Keys.Note2, Octaves.Middle)},
            {$"{Domain.Note.E}{Domain.Octave.Low}", new LuteNote(Keys.Note3, Octaves.Middle)},
            {$"{Domain.Note.F}{Domain.Octave.Low}", new LuteNote(Keys.Note4, Octaves.Middle)},
            {$"{Domain.Note.G}{Domain.Octave.Low}", new LuteNote(Keys.Note5, Octaves.Middle)},
            {$"{Domain.Note.A}{Domain.Octave.Low}", new LuteNote(Keys.Note6, Octaves.Middle)},
            {$"{Domain.Note.B}{Domain.Octave.Low}", new LuteNote(Keys.Note7, Octaves.Middle)},
            {$"{Domain.Note.C}{Domain.Octave.Middle}", new LuteNote(Keys.Note1, Octaves.High)},
            {$"{Domain.Note.D}{Domain.Octave.Middle}", new LuteNote(Keys.Note2, Octaves.High)},
            {$"{Domain.Note.E}{Domain.Octave.Middle}", new LuteNote(Keys.Note3, Octaves.High)},
            {$"{Domain.Note.F}{Domain.Octave.Middle}", new LuteNote(Keys.Note4, Octaves.High)},
            {$"{Domain.Note.G}{Domain.Octave.Middle}", new LuteNote(Keys.Note5, Octaves.High)},
            {$"{Domain.Note.A}{Domain.Octave.Middle}", new LuteNote(Keys.Note6, Octaves.High)},
            {$"{Domain.Note.B}{Domain.Octave.Middle}", new LuteNote(Keys.Note7, Octaves.High)},
            {$"{Domain.Note.C}{Domain.Octave.High}", new LuteNote(Keys.Note8, Octaves.High)}
        };

        public Keys Key { get; }
        public Octaves Octave { get; }

        public LuteNote(Keys key, Octaves octave)
        {
            Key = key;
            Octave = octave;
        }

        public static LuteNote From(BaseNote note)
        {
            return Map[$"{note.Note}{note.Octave}"];
        }

        public override bool Equals(object obj)
        {
            return Equals((LuteNote) obj);
        }

        protected bool Equals(LuteNote other)
        {
            return Key == other.Key && Octave == other.Octave;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key*397) ^ (int) Octave;
            }
        }
    }
}