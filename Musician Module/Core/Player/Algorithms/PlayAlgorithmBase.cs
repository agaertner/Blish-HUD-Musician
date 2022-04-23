using System.Diagnostics;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
    public abstract class PlayAlgorithmBase
    {
        protected bool _abort;

        protected readonly Stopwatch _stopwatch;

        protected PlayAlgorithmBase()
        {
            _stopwatch = new Stopwatch();
        }

        public abstract void Play(InstrumentBase instrument, Metronome metronomeMark, ChordOffset[] melody);

        public virtual void Dispose()
        {
            _abort = true;
            _stopwatch.Stop();
        }
    }
}