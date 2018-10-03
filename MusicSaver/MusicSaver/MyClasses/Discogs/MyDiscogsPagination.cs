// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var welcome = Welcome.FromJson(jsonString);

namespace MusicSaver.MyClasses.Discogs
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    public partial class DiscogsResult
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("per_page")]
        public long PerPage { get; set; }

        [JsonProperty("pages")]
        public long Pages { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("urls")]
        public Urls Urls { get; set; }

        [JsonProperty("items")]
        public long Items { get; set; }
    }

    public partial class Urls
    {
        [JsonProperty("last")]
        public Uri Last { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("style", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Style { get; set; }

        [JsonProperty("thumb")]
        public Uri Thumb { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Format { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("barcode", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Barcode { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("community", NullValueHandling = NullValueHandling.Ignore)]
        public Community Community { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Label { get; set; }

        [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]
        public Genre[] Genre { get; set; }

        [JsonProperty("catno", NullValueHandling = NullValueHandling.Ignore)]
        public string Catno { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? Year { get; set; }

        [JsonProperty("cover_image")]
        public Uri CoverImage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("resource_url")]
        public Uri ResourceUrl { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Community
    {
        [JsonProperty("want")]
        public long Want { get; set; }

        [JsonProperty("have")]
        public long Have { get; set; }
    }

    public enum Genre { Classical, FolkWorldCountry, Jazz, Pop, Rock, StageScreen, Other };

    public enum TypeEnum { Artist, Master, Release };

    public partial class DiscogsResult
    {
        public static DiscogsResult FromJson(string json) => JsonConvert.DeserializeObject<DiscogsResult>(json, MusicSaver.MyClasses.Discogs.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DiscogsResult self) => JsonConvert.SerializeObject(self, MusicSaver.MyClasses.Discogs.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                GenreConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class GenreConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Genre) || t == typeof(Genre?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Classical":
                    return Genre.Classical;
                case "Folk, World, & Country":
                    return Genre.FolkWorldCountry;
                case "Jazz":
                    return Genre.Jazz;
                case "Pop":
                    return Genre.Pop;
                case "Rock":
                    return Genre.Rock;
                case "Stage & Screen":
                    return Genre.StageScreen;
                default:
                return Genre.Other;
            }
            throw new Exception("Cannot unmarshal type Genre");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Genre)untypedValue;
            switch (value)
            {
                case Genre.Classical:
                    serializer.Serialize(writer, "Classical");
                    return;
                case Genre.FolkWorldCountry:
                    serializer.Serialize(writer, "Folk, World, & Country");
                    return;
                case Genre.Jazz:
                    serializer.Serialize(writer, "Jazz");
                    return;
                case Genre.Pop:
                    serializer.Serialize(writer, "Pop");
                    return;
                case Genre.Rock:
                    serializer.Serialize(writer, "Rock");
                    return;
                case Genre.StageScreen:
                    serializer.Serialize(writer, "Stage & Screen");
                    return;
            }
            throw new Exception("Cannot marshal type Genre");
        }

        public static readonly GenreConverter Singleton = new GenreConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "artist":
                    return TypeEnum.Artist;
                case "master":
                    return TypeEnum.Master;
                case "release":
                    return TypeEnum.Release;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Artist:
                    serializer.Serialize(writer, "artist");
                    return;
                case TypeEnum.Master:
                    serializer.Serialize(writer, "master");
                    return;
                case TypeEnum.Release:
                    serializer.Serialize(writer, "release");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
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
