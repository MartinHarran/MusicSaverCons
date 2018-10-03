using MusicSaver.MyClasses.Spotify;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Http;
using SpotifyAPI.Web;

namespace MusicSaver.MyClasses
{
    class Globals
    {
        public static string MYSPOTIFY_CLIENTID = "d1f1a418ade84b4e95268f1606554157";
        public static string MYSPOTIFY_SECRET= "59ed0f3bebb742588c6d11fc4907f0a0";
        public static string MYSPOTIFY_NAME = "mharran";
        public static SpotifyWebAPI SPOTIFY_INST;

        public static Stopwatch TOKEN_TIMER = new Stopwatch();

        public static string CHARTS_PLAYLIST = "2ps7zHd9of81uPSCNZxWEv";
        public static string WAVPATH = @"F:\TestAudio\";
        public static int BITRATE = 320;


        public static string DISCOGS_CONSUMERKEY = "sHUHybJWhwipeMjBswHm";
        public static string DISCOGS_CONSUMERSECRET = "gpGJAgAUYsEdVOjKFKVeaSyzxYFyQMkS";
        public static string DISCOGS_CURRENTTOKEN = "YKYFinugEagnHTsBKgSBsLLyuwdVFAUHRaQwYgdU";


        public static readonly HttpClient WEBCLIENT = new HttpClient();

        public static List<string> errors = new List<string>();

        public static string GetMp3Path(ExtTrack track)
        {
            string path = @"F:\TestAudio\" + track.OCArtist + @"\"+ CleanString(track.Album.Name) ;
            return path;
        }

        
        public static string CleanString(string input)
        {
            input = input.Replace("\"", "'");
            input = input.Replace("\\", "/");
            input = input.Replace("/", "-");
            input = input.Replace("/", "-");
            input = input.Replace(":", "-");
            input = input.Replace("?", "-");
            input = input.Replace("<", "-");
            input = input.Replace(">", "-");
            input = input.Replace("|", "-");
            return input;
        }

    }

}
