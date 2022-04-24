using System;
using System.Threading.Tasks;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
    internal interface ISoundRepository : IDisposable
    {
        Task Initialize();
        SoundEffectInstance Get(GuildWarsControls key, Octave octave);
    }
}
