using System;
using System.Threading.Tasks;

namespace Nekres.Musician.Core.Instrument
{
    internal interface ISoundRepository : IDisposable
    {
        Task Initialize();
    }
}
