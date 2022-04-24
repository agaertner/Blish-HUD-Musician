using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Nekres.Musician.Core.Models
{
    public enum Instrument
    {
        [EnumMember(Value = "bass")]
        Bass,
        [EnumMember(Value = "bell")]
        Bell,
        [EnumMember(Value = "bell2")]
        Bell2,
        [EnumMember(Value = "flute")]
        Flute,
        [EnumMember(Value = "harp")]
        Harp,
        [EnumMember(Value = "horn")]
        Horn,
        [EnumMember(Value = "lute")]
        Lute
    }

    public class MusicSheetBase
    {
        [JsonProperty("guid")]
        public Guid Id { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("instrument"), JsonConverter(typeof(StringEnumConverter))]
        public Instrument Instrument { get; set; }

        public MusicSheetBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
