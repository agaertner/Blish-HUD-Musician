using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
    public class FavorNotesAlgorithm : IPlayAlgorithm
    {
        private bool Abort = false;
        public void Dispose() { Abort = true; }
        public void Play(BaseInstrument instrument, Metronome metronomeMark, ChordOffset[] melody)
        {
            PrepareChordsOctave(instrument, melody[0].Chord);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var strumIndex = 0; strumIndex < melody.Length;)
            {
                if (Abort) return;

                var strum = melody[strumIndex];

                if (stopwatch.ElapsedMilliseconds > metronomeMark.WholeNoteLength.Multiply(strum.Offset).TotalMilliseconds)
                {
                    var chord = strum.Chord;

                    PlayChord(instrument, chord);

                    if (strumIndex < melody.Length - 1)
                    {
                        PrepareChordsOctave(instrument, melody[strumIndex + 1].Chord);
                    }

                    strumIndex++;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
            }

            stopwatch.Stop();
        }

        private void PrepareChordsOctave(BaseInstrument instrument, Chord chord)
        {
            instrument.GoToOctave(chord.Notes.First());
        }

        private void PlayChord(BaseInstrument instrument, Chord chord)
        {
            var notes = chord.Notes.ToArray();

            for (var noteIndex = 0; noteIndex < notes.Length; noteIndex++)
            {
                instrument.PlayNote(notes[noteIndex]);

                if (noteIndex < notes.Length - 1)
                {
                    PrepareNoteOctave(instrument, notes[noteIndex + 1]);
                }
            }
        }

        private void PrepareNoteOctave(BaseInstrument instrument, BaseNote note)
        {
            instrument.GoToOctave(note);
        }
    }
}