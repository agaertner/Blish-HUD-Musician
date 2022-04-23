using System.Collections.Generic;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument.Harp
{
    public class HarpNote
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

        private static readonly Dictionary<string, HarpNote> Map = new Dictionary<string, HarpNote>
        {
            {$"{Domain.Note.C}{Domain.Octave.Lowest}", new HarpNote(Keys.Note1, Octaves.Low)},
            {$"{Domain.Note.D}{Domain.Octave.Lowest}", new HarpNote(Keys.Note2, Octaves.Low)},
            {$"{Domain.Note.E}{Domain.Octave.Lowest}", new HarpNote(Keys.Note3, Octaves.Low)},
            {$"{Domain.Note.F}{Domain.Octave.Lowest}", new HarpNote(Keys.Note4, Octaves.Low)},
            {$"{Domain.Note.G}{Domain.Octave.Lowest}", new HarpNote(Keys.Note5, Octaves.Low)},
            {$"{Domain.Note.A}{Domain.Octave.Lowest}", new HarpNote(Keys.Note6, Octaves.Low)},
            {$"{Domain.Note.B}{Domain.Octave.Lowest}", new HarpNote(Keys.Note7, Octaves.Low)},
            {$"{Domain.Note.C}{Domain.Octave.Low}", new HarpNote(Keys.Note1, Octaves.Middle)},
            {$"{Domain.Note.D}{Domain.Octave.Low}", new HarpNote(Keys.Note2, Octaves.Middle)},
            {$"{Domain.Note.E}{Domain.Octave.Low}", new HarpNote(Keys.Note3, Octaves.Middle)},
            {$"{Domain.Note.F}{Domain.Octave.Low}", new HarpNote(Keys.Note4, Octaves.Middle)},
            {$"{Domain.Note.G}{Domain.Octave.Low}", new HarpNote(Keys.Note5, Octaves.Middle)},
            {$"{Domain.Note.A}{Domain.Octave.Low}", new HarpNote(Keys.Note6, Octaves.Middle)},
            {$"{Domain.Note.B}{Domain.Octave.Low}", new HarpNote(Keys.Note7, Octaves.Middle)},
            {$"{Domain.Note.C}{Domain.Octave.Middle}", new HarpNote(Keys.Note1, Octaves.High)},
            {$"{Domain.Note.D}{Domain.Octave.Middle}", new HarpNote(Keys.Note2, Octaves.High)},
            {$"{Domain.Note.E}{Domain.Octave.Middle}", new HarpNote(Keys.Note3, Octaves.High)},
            {$"{Domain.Note.F}{Domain.Octave.Middle}", new HarpNote(Keys.Note4, Octaves.High)},
            {$"{Domain.Note.G}{Domain.Octave.Middle}", new HarpNote(Keys.Note5, Octaves.High)},
            {$"{Domain.Note.A}{Domain.Octave.Middle}", new HarpNote(Keys.Note6, Octaves.High)},
            {$"{Domain.Note.B}{Domain.Octave.Middle}", new HarpNote(Keys.Note7, Octaves.High)},
            {$"{Domain.Note.C}{Domain.Octave.High}", new HarpNote(Keys.Note8, Octaves.High)}
        };
        public HarpNote(Keys key, Octaves octave)
        {
            Key = key;
            Octave = octave;
        }

        public Keys Key { get; }
        public Octaves Octave { get; }

        public static HarpNote From(BaseNote note)
        {
            return Map[$"{note.Note}{note.Octave}"];
        }

        public override bool Equals(object obj)
        {
            return Equals((HarpNote) obj);
        }

        protected bool Equals(HarpNote other)
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