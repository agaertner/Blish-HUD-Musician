using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
    public abstract class BaseNote
    {
        public readonly GuildWarsControls Key;

        public readonly Octave Octave;

        protected BaseNote(GuildWarsControls key, Octave octave)
        {
            this.Key = key;
            this.Octave = octave;
        }
    }
}
