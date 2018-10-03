// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using MusicSaver.MyClasses.LastFMInfo;
//
//    var GetInfo = GetInfo.FromJson(jsonString);

namespace MusicSaver.MyClasses.LastFMInfo
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetInfo
    {
        [JsonProperty("track")]
        public Track Track { get; set; }
    }

    public partial class Track
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mbid")]
        public Guid Mbid { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Duration { get; set; }

        [JsonProperty("streamable")]
        public Streamable Streamable { get; set; }

        [JsonProperty("listeners")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Listeners { get; set; }

        [JsonProperty("playcount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Playcount { get; set; }

        [JsonProperty("artist")]
        public Artist Artist { get; set; }

        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("toptags")]
        public Toptags Toptags { get; set; }
    }

    public partial class Album
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("mbid")]
        public Guid Mbid { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("image")]
        public Image[] Image { get; set; }

        [JsonProperty("@attr")]
        public Attr Attr { get; set; }
    }

    public partial class Attr
    {
        [JsonProperty("position")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Position { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("#text")]
        public Uri Text { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }
    }

    public partial class Artist
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mbid")]
        public Guid Mbid { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Streamable
    {
        [JsonProperty("#text")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Text { get; set; }

        [JsonProperty("fulltrack")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Fulltrack { get; set; }
    }

    public partial class Toptags
    {
        [JsonProperty("tag")]
        public Tag[] Tag { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class GetInfo
    {
        public static GetInfo FromJson(string json) => JsonConvert.DeserializeObject<GetInfo>(json, MusicSaver.MyClasses.LastFMInfo.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GetInfo self) => JsonConvert.SerializeObject(self, MusicSaver.MyClasses.LastFMInfo.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
