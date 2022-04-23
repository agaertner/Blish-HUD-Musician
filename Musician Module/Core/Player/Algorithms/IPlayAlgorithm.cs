using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
    public interface IPlayAlgorithm
    {
        void Play(BaseInstrument instrument, Metronome metronomeMark, ChordOffset[] melody);
        void Dispose();
    }
}