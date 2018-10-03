using AutoMapper;
using MusicSaver.Models;
using MusicSaver.MyClasses.Discogs;
using MusicSaver.MyClasses.LastFM;
using NAudio.Lame;
using NAudio.MediaFoundation;
using NAudio.Wave;
using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using static MusicSaver.MyClasses.Globals;


namespace MusicSaver.MyClasses.Spotify
{
    class MySpotify
    {
        private static List<string> genres;
        private static DiscogsResult result;

        private static string savePathWav = @"F:\TestAudio\";
        private static string savePathMP3 = savePathWav;
        public static Nullable<long> trackYear = null;
        private static string outputFilePathWav;
        private static string outputFilePathMP3;

        private static WasapiLoopbackCapture captureInstance;
        private static WaveFormat format;
        private static WaveFileWriter recordedAudioWriter;
        private static Token token;

        static AutorizationCodeAuth auth;
        static SpotifyWebAPI instance;

        public static SpotifyWebAPI GetInstanceAuth()
        {

            auth = new AutorizationCodeAuth()
            {
                //Your client Id
                spotifyClientID = MYSPOTIFY_CLIENTID,
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = "http://localhost:8888",
                //How many permissions we need?
                Scope = Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate|Scope.PlaylistModifyPrivate|Scope.PlaylistModifyPublic | Scope.UserLibraryRead |
                    Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                    Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState,
            };
            //This will be called, if the user cancled/accept the auth-request
            auth.OnResponseReceivedEvent += auth_OnResponseReceivedEvent;
            //a local HTTP Server will be started (Needed for the response)
            auth.StartHttpServer();
            //This will open the spotify auth-page. The user can decline/accept the request
            auth.DoAuth();

            Thread.Sleep(10000);

            return SPOTIFY_INST;

        }

        private static void auth_OnResponseReceivedEvent(AutorizationCodeAuthResponse response)
        {

            //NEVER DO THIS! You would need to provide the ClientSecret.
            //You would need to do it e.g via a PHP-Script.
            token = auth.ExchangeAuthCode(response.Code, MYSPOTIFY_SECRET);

            SPOTIFY_INST = new SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken
            };

            //With the token object, you can now make API calls

            //Stop the HTTP Server, done.
            auth.StopHttpServer();
        }



        public static Token RefreshToken( )
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Authorization",
                    "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(MYSPOTIFY_CLIENTID + ":" + MYSPOTIFY_SECRET)));
                NameValueCollection col = new NameValueCollection
                {
                    {"grant_type", "refresh_token"},
                    {"refresh_token", token.RefreshToken}
                };

                string response;
                try
                {
                    byte[] data = wc.UploadValues("https://accounts.spotify.com/api/token", "POST", col);
                    response = Encoding.UTF8.GetString(data);
                }
                catch (WebException e)
                {
                    using (StreamReader reader = new StreamReader(e.Response.GetResponseStream()))
                    {
                        response = reader.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeObject<Token>(response);
            }
        }

        public static SpotifyWebAPI GetInstance()
        {
            SPOTIFY_INST = new SpotifyWebAPI();
            AuthoriseAsync();
            return SPOTIFY_INST;

        }
        public static async Task AuthoriseAsync()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
               "http://localhost",
               8888,
               MYSPOTIFY_CLIENTID,
               Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.PlaylistModifyPrivate | Scope.PlaylistModifyPublic | Scope.UserLibraryRead |
                    Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                    Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState,
               TimeSpan.FromSeconds(20)
          );

            try
            {
                //This will open the user's browser and returns once
                //the user is authorized.
                SPOTIFY_INST = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (SPOTIFY_INST == null)
                return;
        }

        public static void TrackFromUrl (String chartUrl)
        {


/*            string url = "https://api.discogs.com/database/search?" + HttpUtility.UrlEncode(extTrack.OCName);
            if (extTrack.OCArtist != "") url += "&artist=" + HttpUtility.UrlEncode(extTrack.OCArtist);
            url += "&per_page=100&type=master";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Headers["Authorization"] = "Discogs key=" + discogsConsumerKey + ", secret=" + discogsConsumerSecret;
            myReq.UserAgent = "MyGenres/1.0 +http://martinharran.com";
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
*/
        }

        public static void PlayAndRecord(Record_Spotify toRecord)
        {

            SpotifyWebAPI spotify = SPOTIFY_INST;
            if (spotify.AccessToken == null)
            {
                spotify = GetInstanceAuth();
            }
            ErrorResponse error = spotify.PausePlayback();
            string spotifyID = toRecord.SpotifyURL.Substring(31, 22);
            FullTrack track = spotify.GetTrack(spotifyID);

            if (track.Uri == null)
            {
                MyRequests.GetTrack(spotifyID);
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<FullTrack, ExtTrack>());
            var mapper = config.CreateMapper();
            ExtTrack extTrack = mapper.Map<ExtTrack>(track);
            extTrack.AddProps(toRecord.TrackTitle, toRecord.ArtistName, toRecord.TrackID);

            outputFilePathWav = savePathWav + CleanString(extTrack.Filename) + ".wav"; 
            outputFilePathMP3 = savePathMP3 + CleanString(extTrack.Filename) + ".mp3";

            error = spotify.ResumePlayback("", "", new List<string> { track.Uri.ToString() }, 0);



            Record_Start(outputFilePathWav);
            System.Threading.Thread.Sleep(track.DurationMs + 1000);
            //System.Threading.Thread.Sleep(10000);
            error = spotify.PausePlayback();
            Record_Stop();

            WaveToMP3(outputFilePathWav, outputFilePathMP3, extTrack);
        }

        private static void Record_Start(string outputFilePath)
        {


            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            captureInstance = new WasapiLoopbackCapture();
            format = new WaveFormat(44100, 2);

            // Redefine the audio writer instance with the given configuration
            recordedAudioWriter = new WaveFileWriter(outputFilePath, captureInstance.WaveFormat);

            // When the capturer receives audio, start writing the buffer into the mentioned file
            captureInstance.DataAvailable += (s, a) =>
            {
                recordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };


            // Start recording !
            captureInstance.StartRecording();

        }

        private static void Record_Stop()
        {
            {
                // Stop recording !
                captureInstance.StopRecording();
                try
                {
                    recordedAudioWriter.Dispose();
                    recordedAudioWriter = null;
                    captureInstance.Dispose();

                }
                catch (Exception e)
                {
                }

            }
        }


        private static void WaveToMP3(string waveFileName, string mp3FileName, ExtTrack extTrack, int bitRate = 320)
        {
            //get genres

            genres = new List<string>();
            DiscogCall(extTrack);
            string directory = savePathMP3 + "\\" + CleanString(extTrack.TagArtists[0]) + "\\" + CleanString(extTrack.Album.Name);
            string fullPathMP3 = directory +"\\" + CleanString(extTrack.Filename) + ".mp3";
            Directory.CreateDirectory(directory);

            // LASTFM - genres dodgy?
            // extTrack.Year = LastFMCall(extTrack.OCName, extTrack.OCArtist);

            extTrack.TagGenres = genres.ToArray();
 
            // temporary workaround for problems with genres
            extTrack.TagGenres = new string[] { string.Join(",", extTrack.TagGenres) };

            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            using (var reader = new AudioFileReader(waveFileName))
            using (var writer = new LameMP3FileWriter(fullPathMP3, reader.WaveFormat, bitRate))
                reader.CopyTo(writer);
            //timer.Stop();
            //long elapsedSecs = timer.ElapsedMilliseconds / 1000;


            //tag wav file
            TagLib.File wav = TagLib.File.Create(waveFileName);
            wav.Tag.Title = extTrack.Filename;
            wav.Tag.Performers = extTrack.TagArtists;
            wav.Tag.Album = extTrack.Album.Name;
            wav.Tag.AlbumArtists = extTrack.TagArtists;
            wav.Tag.Genres = extTrack.TagGenres;
            //wav.Tag.Comment = "my comments, maybe some info about track";
            if (extTrack.Year!=null) wav.Tag.Year = (uint) extTrack.Year;

            wav.Save();
            wav.Dispose();

            //tag mp3 file
            TagLib.File mp3 = TagLib.File.Create(fullPathMP3);
            mp3.Tag.Title = extTrack.Filename;
            mp3.Tag.Performers = extTrack.TagArtists;
            mp3.Tag.Album = extTrack.Album.Name;
            mp3.Tag.AlbumArtists = extTrack.TagArtists;
            mp3.Tag.Genres = extTrack.TagGenres;
            //mp3.Tag.Comment = "my comments, maybe some info about track";
            if (extTrack.Year != null) mp3.Tag.Year = (uint)extTrack.Year;

            mp3.Save();
            mp3.Dispose();

            // update table with filepath of mp3.
            OfficialChartsEntities db2 = new OfficialChartsEntities();
            var record = db2.Tracks.SingleOrDefault(b => b.TrackID == extTrack.OCID);
            if (record != null)
            {
                record.PathOnDisc = fullPathMP3;
                db2.SaveChanges();
            }
            // CheckFileTags(extTrack.Filename);
        }


        public static void WaveToMP3MF(string inputFile, string outputFile)
        {
             MediaFoundationApi.Startup();


           //var mediaType = MediaFoundationEncoder.SelectMediaType(
            //        AudioSubtypes.MFAudioFormat_MP3,
            //        new WaveFormat(48000, 1),
            //        0);

            //if (mediaType != null) { } // we can encode… 


            using (var reader = new WaveFileReader(inputFile))
            {
                MediaFoundationEncoder.EncodeToMp3(reader,
                         outputFile, 0);
            }

        }
        private static void DiscogCall(ExtTrack extTrack)
        {

            string url = "https://api.discogs.com/database/search?" + System.Web.HttpUtility.UrlEncode(extTrack.OCName);
            if (extTrack.OCArtist!= "") url+= "&artist=" + HttpUtility.UrlEncode(extTrack.OCArtist);
            url += "&per_page=100&type=master";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Headers["Authorization"] = "Discogs key=" + DISCOGS_CONSUMERKEY + ", secret=" + DISCOGS_CONSUMERSECRET;
            myReq.UserAgent = "MyGenres/1.0 +http://martinharran.com";
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();

            result = DiscogsResult.FromJson(content);

            if (result != null)
            {
                genres = new List<string>();
                if (result.Results != null)
                {
                    for (int i = 0; i < result.Results.Count(); i++)
                    {
                        Result r = result.Results[i];
                        if(r.Country != null)
                        {
                            if (r.Country.ToUpper()=="UK"
                                || r.Country.ToUpper() == "US"
                                || r.Country.ToUpper() == "Europe"
                                )
                            {
                                if (r.Style != null)
                                {
                                    for (int g = 0; g < r.Style.Count(); g++)
                                    {
                                        string style = r.Style[g];
                                        //string style = r.Style[g].ToString().Replace(" ", "_");

                                        var match = genres
                                            .FirstOrDefault(stringToCheck => stringToCheck.Contains(style));

                                        if (match == null)
                                        {
                                            genres.Add(style); }
                                    }
                                }

                            }
                        }
                        if(r.Year!=null)
                        {
                            if (extTrack.Year == null)  extTrack.Year = (int)r.Year;
                            else if (r.Year < trackYear) extTrack.Year = (int) r.Year;
                        }

                    }
                }


            }


        }

        private static Nullable<int> LastFMCall(string title, string artist = "")
        {
            Nullable<int> year = MyLastFM.SearchTrack(ref genres, title, artist);
            return year;
        }

        public static void CheckFileTags(string fileName)
        {
            //TagLib.File wav = TagLib.File.Create(savePathWav+fileName+".wav");
            //wav.Dispose();

            //tag mp3 file
            TagLib.File mp3 = TagLib.File.Create(savePathMP3 + fileName + ".mp3");
            string[] artists = new string[] { "artist 1", "artist 2", "artist 3", "artist 4" } ;
            mp3.Tag.AlbumArtists = artists;
            string[] performers = new string[]{ "Jim Reeves", "Patsy Cline", "\"Boxcar\" Willie" };
            mp3.Tag.Performers = performers;
            mp3.Save();
            mp3.Dispose();

        }

        public static void ReduceChannels(string track = @"F:\TestAudio\\The Hollies\50 At Fifty\If I Needed Someone_Hollies.mp3")
        {
            int outRate = 48000;
            var inFile = @"F:\TestAudio\If I Needed Someone_Hollies.wav";
            var outFile = @"F:\TestAudio\resampled.wav";
            using (var reader = new AudioFileReader(inFile))
            {
                var outFormat = new WaveFormat(outRate, 2);
                using (var resampler = new MediaFoundationResampler(reader, outFormat))
                {
                    // resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(outFile, resampler);
                }
            }
            WaveToMP3MF(outFile, track);
        }

        public static string IdFromUrl ( string url)
        {
            int pos = url.LastIndexOf('/');
            return url.Substring(pos+1).Trim();
        }
    }
}
