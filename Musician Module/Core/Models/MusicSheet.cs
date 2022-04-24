using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Domain.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Nekres.Musician.Core.Models
{
    public enum Algorithm
    {
        [EnumMember(Value = "favor-notes")]
        FavorNotes,
        [EnumMember(Value = "favor-chords")]
        FavorChords
    }

    internal class MusicSheet : MusicSheetBase
    {
        [JsonProperty("tempo"), JsonConverter(typeof(MetronomeConverter))]
        public Metronome Tempo { get; set; }

        [JsonProperty("algorithm"), JsonConverter(typeof(StringEnumConverter))]
        public Algorithm? Algorithm { get; set; }

        [JsonProperty("melody"), JsonConverter(typeof(ChordOffsetConverter))]
        public IEnumerable<ChordOffset> Melody { get; set; }

        public MusicSheetBase GetBaseInfo()
        {
            return new MusicSheetBase
            {
                Id = this.Id,
                Artist = this.Artist,
                Title = this.Title,
                User = this.User,
                Instrument = this.Instrument
            };
        }

        public static MusicSheet FromXml(string path)
        {
            var timeout = DateTime.UtcNow.AddMilliseconds(FileUtil.FileTimeOutMilliseconds);
            while (DateTime.UtcNow < timeout)
            {
                try
                {
                    var xDocument = XDocument.Load(path);

                    var title = xDocument.Elements().SingleOrDefault()?.Elements("title").SingleOrDefault()?.Value ?? "???";
                    var artist = xDocument.Elements().SingleOrDefault()?.Elements("artist").SingleOrDefault()?.Value ?? "Unknown Artist";
                    var user = xDocument.Elements().SingleOrDefault()?.Elements("user").SingleOrDefault()?.Value ?? string.Empty;
                    
                    if (!Enum.TryParse<Instrument>(xDocument.Elements().SingleOrDefault()?.Elements("instrument").Single().Value, out var instrument))
                        return null;

                    var tempo = xDocument.Elements().SingleOrDefault()?.Elements("tempo").SingleOrDefault()?.Value;
                    var meter = xDocument.Elements().SingleOrDefault()?.Elements("meter").SingleOrDefault()?.Value;

                    if (string.IsNullOrEmpty(tempo) || string.IsNullOrEmpty(meter))
                        return null;

                    var melody = xDocument.Elements().SingleOrDefault()?.Elements("melody").SingleOrDefault()?.Value;
                    if (string.IsNullOrEmpty(melody))
                        return null;

                    if (!Enum.TryParse<Algorithm>(xDocument.Elements().SingleOrDefault()?.Elements("algorithm").Single().Value.Replace(" ", string.Empty), out var algorithm))
                        return null;

                    return new MusicSheet
                    {
                        Title = title,
                        Artist = artist,
                        User = user,
                        Instrument = instrument,
                        Tempo = Metronome.FromString($"{tempo} {meter}"),
                        Melody = ChordOffset.MelodyFromString(melody),
                        Algorithm = algorithm
                    };
                }
                catch (Exception e) when (e is IOException or UnauthorizedAccessException or XmlException)
                {
                    if (DateTime.UtcNow < timeout && e.GetType() != typeof(XmlException)) continue;
                    MusicianModule.Logger.Warn(e, e.Message);
                }
            }
            return null;
        }
    }
}