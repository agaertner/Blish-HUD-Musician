using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Domain.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nekres.Musician.Core.Models
{
    internal class MusicSheet : MusicSheetBase
    {
        [JsonProperty("instrument"), JsonConverter(typeof(StringEnumConverter))]
        public Instrument Instrument { get; set; }

        [JsonProperty("tempo"), JsonConverter(typeof(MetronomeConverter))]
        public Metronome Tempo { get; set; }

        [JsonProperty("algorithm"), JsonConverter(typeof(StringEnumConverter))]
        public Algorithm? Algorithm { get; set; }

        [JsonProperty("melody"), JsonConverter(typeof(ChordOffsetConverter))]
        public IEnumerable<ChordOffset> Melody { get; set; }
    }
}