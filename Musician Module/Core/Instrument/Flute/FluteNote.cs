using System.Collections.Generic;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument.Flute
{
    public class FluteNote
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
            High
        }

        private static readonly Dictionary<string, FluteNote> Map = new Dictionary<string, FluteNote>
        {
            // Low Octave
            {$"{Domain.Note.E}{Domain.Octave.Low}", new FluteNote(Keys.Note1, Octaves.Low)},
            {$"{Domain.Note.F}{Domain.Octave.Low}", new FluteNote(Keys.Note2, Octaves.Low)},
            {$"{Domain.Note.G}{Domain.Octave.Low}", new FluteNote(Keys.Note3, Octaves.Low)},
            {$"{Domain.Note.A}{Domain.Octave.Low}", new FluteNote(Keys.Note4, Octaves.Low)},
            {$"{Domain.Note.B}{Domain.Octave.Low}", new FluteNote(Keys.Note5, Octaves.Low)},
            {$"{Domain.Note.C}{Domain.Octave.Middle}", new FluteNote(Keys.Note6, Octaves.Low)},
            {$"{Domain.Note.D}{Domain.Octave.Middle}", new FluteNote(Keys.Note7, Octaves.Low)},
            {$"{Domain.Note.E}{Domain.Octave.Middle}", new FluteNote(Keys.Note8, Octaves.Low)},
            {$"{Domain.Note.F}{Domain.Octave.Middle}", new FluteNote(Keys.Note2, Octaves.High)},
            {$"{Domain.Note.G}{Domain.Octave.Middle}", new FluteNote(Keys.Note3, Octaves.High)},
            {$"{Domain.Note.A}{Domain.Octave.Middle}", new FluteNote(Keys.Note4, Octaves.High)},
            {$"{Domain.Note.B}{Domain.Octave.Middle}", new FluteNote(Keys.Note5, Octaves.High)},
            {$"{Domain.Note.C}{Domain.Octave.High}", new FluteNote(Keys.Note6, Octaves.High)},
            {$"{Domain.Note.D}{Domain.Octave.High}", new FluteNote(Keys.Note7, Octaves.High)},
            {$"{Domain.Note.E}{Domain.Octave.High}", new FluteNote(Keys.Note8, Octaves.High)}
        };

        public Keys Key { get; }
        public Octaves Octave { get; }

        public FluteNote(Keys key, Octaves octave)
        {
            Key = key;
            Octave = octave;
        }

        public static FluteNote From(BaseNote note)
        {
            return Map[$"{note.Note}{note.Octave}"];
        }

        public override bool Equals(object obj)
        {
            return Equals((FluteNote) obj);
        }

        protected bool Equals(FluteNote other)
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