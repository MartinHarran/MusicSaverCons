using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MusicSaver.MyClasses;

namespace MusicSaver.MyClasses.LastFM
{
    class MyLastFM
    {
        //static string lfmApplicationName = "My Genres";
        static string lfmAPIkey = "a7d3141236a6a9ed202d218a63caead3";
        //static string lfmSharedSecret = "93592a9fd85491c51a95757d4eaa21d8";
        static string lfmRootUrl = "http://ws.audioscrobbler.com/2.0/";


        public static Nullable<int> SearchTrack(ref List<string> genres, string track, string artist = "")
        {
  
            Nullable<int> year = null;

            string url = lfmRootUrl + "?method=track.search&track=" + HttpUtility.UrlEncode(track);
            if (artist != "") url += "&artist=" + HttpUtility.UrlEncode(artist);
            url += "&api_key=" + lfmAPIkey + "&format=json";


            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.UserAgent = "MyGenres/1.0 +http://martinharran.com";
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();

            LastFMResult result = LastFMResult.FromJson(content);

            if (result.Results != null)
            {
                for (int c = 0; c < result.Results.Trackmatches.Track.Count(); c++)
                {
                    //LastFm Track Info
                    LastFM.Track t = result.Results.Trackmatches.Track[c];
                    string infoUrl = lfmRootUrl + "?method=track.getinfo&api_key=" + lfmAPIkey + "&mbid=" + t.Mbid + "&format=json"; ;
                    HttpWebRequest infoReq = (HttpWebRequest)WebRequest.Create(infoUrl);
                    infoReq.UserAgent = "MyGenres/1.0 +http://martinharran.com";
                    WebResponse infoWr = infoReq.GetResponse();
                    Stream infoReceiveStream = infoWr.GetResponseStream();
                    StreamReader infoReader = new StreamReader(infoReceiveStream, Encoding.UTF8);
                    String infoContent = infoReader.ReadToEnd();
                    LastFMInfo.GetInfo infoResult = LastFMInfo.GetInfo.FromJson(infoContent);

                    if (infoResult.Track!=null && infoResult.Track.Toptags != null)
                    {
                        for (int tagNo = 0; tagNo < infoResult.Track.Toptags.Tag.Count(); tagNo++)
                        {
                            LastFMInfo.Tag style = infoResult.Track.Toptags.Tag[tagNo];
                            if (genres == null) genres = new List<string>();
                            string lfmStyle = style.Name;

                            var match = genres
                                .FirstOrDefault(stringToCheck => stringToCheck.Contains(lfmStyle));

                            if (match == null) { genres.Add(lfmStyle); }
                        }
                    }


                }


            }

            return year;

        }

    }

    public partial class LastFMResult
    {
        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("opensearch:Query")]
        public OpensearchQuery OpensearchQuery { get; set; }

        [JsonProperty("opensearch:totalResults")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OpensearchTotalResults { get; set; }

        [JsonProperty("opensearch:startIndex")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OpensearchStartIndex { get; set; }

        [JsonProperty("opensearch:itemsPerPage")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long OpensearchItemsPerPage { get; set; }

        [JsonProperty("trackmatches")]
        public Trackmatches Trackmatches { get; set; }

        [JsonProperty("@attr")]
        public Attr Attr { get; set; }
    }

    public partial class Attr
    {
    }

    public partial class OpensearchQuery
    {
        [JsonProperty("#text")]
        public string Text { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("startPage")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long StartPage { get; set; }
    }

    public partial class Trackmatches
    {
        [JsonProperty("track")]
        public Track[] Track { get; set; }
    }

    public partial class Track
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("streamable")]
        public Streamable Streamable { get; set; }

        [JsonProperty("listeners")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Listeners { get; set; }

        [JsonProperty("image")]
        public Image[] Image { get; set; }

        [JsonProperty("mbid")]
        public string Mbid { get; set; }


    }

    public partial class Image
    {
        [JsonProperty("#text")]
        public Uri Text { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; }
    }

    public enum Size { Extralarge, Large, Medium, Small };

    public enum Streamable { Fixme };

    public partial class LastFMResult
    {
        public static LastFMResult FromJson(string json) => JsonConvert.DeserializeObject<LastFMResult>(json, MusicSaver.MyClasses.Discogs.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LastFMResult self) => JsonConvert.SerializeObject(self, MusicSaver.MyClasses.Discogs.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                SizeConverter.Singleton,
                StreamableConverter.Singleton,
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

    internal class SizeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Size) || t == typeof(Size?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "extralarge":
                    return Size.Extralarge;
                case "large":
                    return Size.Large;
                case "medium":
                    return Size.Medium;
                case "small":
                    return Size.Small;
            }
            throw new Exception("Cannot unmarshal type Size");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Size)untypedValue;
            switch (value)
            {
                case Size.Extralarge:
                    serializer.Serialize(writer, "extralarge");
                    return;
                case Size.Large:
                    serializer.Serialize(writer, "large");
                    return;
                case Size.Medium:
                    serializer.Serialize(writer, "medium");
                    return;
                case Size.Small:
                    serializer.Serialize(writer, "small");
                    return;
            }
            throw new Exception("Cannot marshal type Size");
        }

        public static readonly SizeConverter Singleton = new SizeConverter();
    }

    internal class StreamableConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Streamable) || t == typeof(Streamable?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "FIXME")
            {
                return Streamable.Fixme;
            }
            throw new Exception("Cannot unmarshal type Streamable");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Streamable)untypedValue;
            if (value == Streamable.Fixme)
            {
                serializer.Serialize(writer, "FIXME");
                return;
            }
            throw new Exception("Cannot marshal type Streamable");
        }

        public static readonly StreamableConverter Singleton = new StreamableConverter();

    }
}
