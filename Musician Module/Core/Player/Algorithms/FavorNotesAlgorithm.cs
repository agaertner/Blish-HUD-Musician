using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
    public class FavorNotesAlgorithm : PlayAlgorithmBase
    {
        public override void Play(InstrumentBase instrument, Metronome metronomeMark, ChordOffset[] melody)
        {
            PrepareChordsOctave(instrument, melody[0].Chord);

            _stopwatch.Start();

            for (var strumIndex = 0; strumIndex < melody.Length;)
            {
                if (_abort) return;

                var strum = melody[strumIndex];

                if (_stopwatch.ElapsedMilliseconds > metronomeMark.WholeNoteLength.Multiply(strum.Offset).TotalMilliseconds)
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

            this.Dispose();
        }

        private void PrepareChordsOctave(InstrumentBase instrument, Chord chord)
        {
            instrument.GoToOctave(chord.Notes.First());
        }

        private void PlayChord(InstrumentBase instrument, Chord chord)
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

        private void PrepareNoteOctave(InstrumentBase instrument, RealNote note)
        {
            instrument.GoToOctave(note);
        }
    }
}