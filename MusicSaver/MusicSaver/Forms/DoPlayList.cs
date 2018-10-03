using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MusicSaver.MyClasses;
using static MusicSaver.MyClasses.Globals;
using MusicSaver.MyClasses.Discogs;
using MusicSaver.MyClasses.LastFM;
using MusicSaver.MyClasses.Spotify;
using MusicSaver.Models;

using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses

using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Lame;

using TagLib;
using TagLib.Id3v2;
using AutoMapper;

using NAudio.Lame;
using NAudio.MediaFoundation;
using NAudio.Wave;

using AutoMapper;

using System.Diagnostics;




namespace MusicSaver.Forms
{
    public partial class FullPlayList : Form
    {
        private static SpotifyWebAPI spotify;
        private static Stopwatch stopwatch = new Stopwatch();
        static Paging<PlaylistTrack> playlist;
        static FullTrack fullTrack;
        static SimplePlaylist thisPlayList;
        static Paging<SimplePlaylist> playlists;
        static ErrorResponse error;

        private static WasapiLoopbackCapture captureInstance1;
        private static WaveFormat format1;
        private static WaveFileWriter recordedAudioWriter1;
        private static WasapiLoopbackCapture captureInstance2;
        private static WaveFormat format2;
        private static WaveFileWriter recordedAudioWriter2;

        private static FullTrack lastTrack1 = null;
        private static FullTrack thisTrack1 = null;
        private static FullTrack lastTrack2 = null;
        private static FullTrack thisTrack2 = null;
        private static FullTrack lastTrack = null;
        private static FullTrack thisTrack = null;


        private static Thread tRecord;
        private static Thread tMP3;
        private static int whichFile =1;
        private static int fileToCopy;
        private static int spotifyLoadTime = 5;

        private static long elapsedTime = 0;
        private static int tokenElapseTime=1;

        public FullPlayList()
        {
            InitializeComponent();
            GetPlayLists();
        }

        private void bRefreshPlaylists_Click(object sender, EventArgs e)
        {
            GetPlayLists();
        }

        private void CheckToken()
        {
            if (SPOTIFY_INST == null)
            {
                SPOTIFY_INST = MySpotify.GetInstanceAuth();
                TOKEN_TIMER.Restart();

            }
            else
            {
                if (TOKEN_TIMER.ElapsedMilliseconds > (tokenElapseTime * 60 * 1000))
                {
                    //SPOTIFY_INST.AccessToken = MySpotify.RefreshToken().AccessToken;
                    MySpotify.RefreshToken();
                    TOKEN_TIMER.Restart();
                }

            }
            spotify = SPOTIFY_INST;

        }

        private void GetPlayLists()
        {
            CheckToken();
            playlists = spotify.GetUserPlaylists(MYSPOTIFY_NAME);

            // dgvPlaylists.DataSource = userPlaylists.Items;
            dgvPlaylists.Rows.Clear();

            for (int c= 0; c< playlists.Items.Count; c++)
            {
                dgvPlaylists.Rows.Add(playlists.Items[c].Name, 
                    playlists.Items[c].Id, 
                    playlists.Items[c].Tracks.Total, 
                    playlists.Items[c].Uri
                    );

            }
        }

        private void dgvPlaylists_SelectionChanged(object sender, EventArgs e)
        {
            GetTracks();
        }

        private void GetTracks()
        {
            if (SPOTIFY_INST == null)
            {
                SPOTIFY_INST = MySpotify.GetInstanceAuth();
                stopwatch.Start();

            }
            else
            {
                if (stopwatch.ElapsedMilliseconds > (55 * 60 * 1000))
                {
                    SPOTIFY_INST.AccessToken = MySpotify.RefreshToken().AccessToken;
                    stopwatch.Reset();
                }

            }
            spotify = SPOTIFY_INST;

            playlist = spotify.GetPlaylistTracks(MYSPOTIFY_NAME, dgvPlaylists.CurrentRow.Cells[1].Value.ToString());
            dgvTracksOnList.Rows.Clear();
            for (int c = 0; c < playlist.Items.Count; c++)
            {
                fullTrack = playlist.Items[c].Track;
                dgvTracksOnList.Rows.Add(fullTrack.Name,
                fullTrack.Artists[0].Name,
                fullTrack.Id,
                fullTrack.Uri,
                fullTrack.DurationMs
                );

            }
        }


        private void bPlayAndRecord_Click(object sender, EventArgs e)
        {
            if (dgvPlaylists.CurrentRow.Index < 0) return;

            thisPlayList = playlists.Items[dgvPlaylists.CurrentRow.Index];
            lastTrack1 = null; lastTrack2 = null;
            for (int t = 0; t < playlist.Items.Count; t++)
            {
             error = spotify.ResumePlayback(contextUri: "spotify:user:"+ MYSPOTIFY_NAME+":playlist:" + thisPlayList.Id, offset: t);
               if (whichFile == 1)
                {
                thisTrack1 = playlist.Items[t].Track;
                Thread tRecord = new Thread(RecordTrack);
                tRecord.Start();
                tRecord.Join();

                }
                else
                {
                    thisTrack2 = playlist.Items[t].Track;
                    Thread tRecord = new Thread(RecordTrack);
                    tRecord.Start();
                    tRecord.Join();

                }

            }
        }

        private void RecordTrack()
        {
            //start recording
            if (whichFile == 1)
            {
                elapsedTime += thisTrack1.DurationMs;
                captureInstance1 = new WasapiLoopbackCapture();
                //format = new WaveFormat(44100, 2);
                recordedAudioWriter1 = new WaveFileWriter(WAVPATH + (whichFile == 1 ? "thisTrack1.wav" : "thisTrack2.wav"), captureInstance1.WaveFormat);

                // Handle data not yet available
                Stopwatch dataTimer = new Stopwatch();
                dataTimer.Start();
                captureInstance1.DataAvailable += (s, a) =>
                {
                    recordedAudioWriter1.Write(a.Buffer, 0, a.BytesRecorded);
                };

                // Start recording !
                Stopwatch timer1 = new Stopwatch();
                timer1.Start();
                captureInstance1.StartRecording();
                //record this track
                while (timer1.ElapsedMilliseconds < thisTrack1.DurationMs)
                {
                    //wait for this track to finish
                }
                error = spotify.PausePlayback();
                timer1.Stop();
                //finish recording; start next
                Stopwatch processTimer1 = new Stopwatch();
                captureInstance1.StopRecording();
                try
                {
                    recordedAudioWriter1.Dispose();
                    recordedAudioWriter1 = null;
                    captureInstance1.Dispose();

                }
                catch (Exception err)
                {
                }
                fileToCopy = 1;
                whichFile = 2;
                CopyLastTrack();

                Thread tMP3 = new Thread(CopyToMP3);
                tMP3.Start();
            }
            else
            {
                elapsedTime += thisTrack2.DurationMs;
                captureInstance2 = new WasapiLoopbackCapture();
                //format = new WaveFormat(44100, 2);
                recordedAudioWriter2 = new WaveFileWriter(WAVPATH + (whichFile == 1 ? "thisTrack1.wav" : "thisTrack2.wav"), captureInstance2.WaveFormat);

                captureInstance2.DataAvailable += (s, a) =>
                {
                    recordedAudioWriter2.Write(a.Buffer, 0, a.BytesRecorded);
                };

                // Start recording !
                Stopwatch timer2 = new Stopwatch();
                timer2.Start();
                captureInstance2.StartRecording();
                //record this track
                while (timer2.ElapsedMilliseconds < thisTrack2.DurationMs)
                {
                    //wait for this track to finish
                }
                //finish recording; start next
                error = spotify.PausePlayback();
                timer2.Stop();
                captureInstance2.StopRecording();
                try
                {
                    recordedAudioWriter2.Dispose();
                    recordedAudioWriter2 = null;
                    captureInstance2.Dispose();

                }
                catch (Exception err)
                {
                }
                fileToCopy = 2;
                whichFile = 1;
                CopyLastTrack();
                Thread tMP3 = new Thread(CopyToMP3);
                tMP3.Start();

            }
        }

         private void CopyLastTrack()
        {
            System.IO.File.Copy(WAVPATH + (fileToCopy == 1 ? "thisTrack1.wav" : "thisTrack2.wav"), WAVPATH + "lastTrack.wav", true);
            thisTrack = (fileToCopy == 1 ? thisTrack1 : thisTrack2);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<FullTrack, FullTrack>());
            var mapper = config.CreateMapper();
            lastTrack = mapper.Map<FullTrack>(thisTrack);
        }

        private void CopyToMP3()
        {
            // Time to clear lock
            Thread.Sleep(10000);


            if (lastTrack != null)
            {
                //Save last track to mp3

                var configET = new MapperConfiguration(cfg => cfg.CreateMap<FullTrack, ExtTrack>());
                var mapperET = configET.CreateMapper();
                string name = CleanString(lastTrack.Name);
                int remaster = name.IndexOf("- Remaster", StringComparison.CurrentCultureIgnoreCase);
                if (remaster > 0)
                {
                    name = name.Substring(0, remaster - 1);
                }
                else
                {
                    remaster = name.IndexOf("/ Remaster", StringComparison.CurrentCultureIgnoreCase);
                    if (remaster > 0)
                    {
                        name = name.Substring(0, remaster - 1);
                    }

                }

                remaster = name.IndexOf("Remaster", StringComparison.CurrentCultureIgnoreCase);
                if (remaster > 0)
                {
                    name = name.Substring(0, remaster - 1);
                }
                string artist = CleanString(lastTrack.Artists[0].Name);


                ExtTrack extTrack = mapperET.Map<ExtTrack>(lastTrack);
                extTrack.AddProps(name, artist, "");

                
                List<string> genres = DiscogCall(extTrack, txtAddGenres.Text);

                string directory = GetMp3Path(extTrack);
                string fullPathMP3 = directory + @"\" + CleanString(extTrack.Filename) + ".mp3";
                Directory.CreateDirectory(directory);

                // LASTFM - genres dodgy?
                // extTrack.Year = LastFMCall(extTrack.OCName, extTrack.OCArtist);

                extTrack.TagGenres = genres.ToArray();

                // temporary workaround for problems with genres
                extTrack.TagGenres = new string[] { string.Join(",", extTrack.TagGenres) };

                using (var reader = new AudioFileReader(WAVPATH + (fileToCopy == 1 ? "thisTrack1.wav" : "thisTrack2.wav")))
                using (var writer = new LameMP3FileWriter(fullPathMP3, reader.WaveFormat, BITRATE))
                    reader.CopyTo(writer);


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

           }
            
        }

        private static List<string> DiscogCall(ExtTrack extTrack, string addGenre = "")
        {

            string url = "https://api.discogs.com/database/search?" + HttpUtility.UrlEncode(extTrack.OCName);
            if (extTrack.OCArtist != "") url += "&artist=" + HttpUtility.UrlEncode(extTrack.OCArtist);
            url += "&per_page=100&type=master";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Headers["Authorization"] = "Discogs key=" + DISCOGS_CONSUMERKEY + ", secret=" + DISCOGS_CONSUMERSECRET;
            myReq.UserAgent = "MyGenres/1.0 +http://martinharran.com";
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            int trackYear = 3000;

            DiscogsResult result = DiscogsResult.FromJson(content);
            List<string> genres = new List<string>();
            if (addGenre != "")
            {
                genres.Add(addGenre);
            }

            if (result != null)
            {
                if (result.Results != null)
                {
                    for (int i = 0; i < result.Results.Count(); i++)
                    {
                        Result r = result.Results[i];
                        if (r.Country != null)
                        {
                            if (r.Country.ToUpper() == "UK"
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

                                        var match = genres.FirstOrDefault(stringToCheck => stringToCheck.Contains(style));

                                        if (match == null)
                                        {
                                            genres.Add(style);
                                        }
                                    }
                                }

                            }
                        }
                        if (r.Year != null)
                        {
                            if (extTrack.Year == null) extTrack.Year = (int)r.Year;
                            else if (r.Year < trackYear) extTrack.Year = (int)r.Year;
                        }

                    }
                }


            }

            return genres;
        }

        private void bUKUpdate_Click(object sender, EventArgs e)
        {
            UKUpdate();
        }

        private void UKUpdate()
        {
            CheckToken();
            int start = 0, end = 0;
            if (txtUKStart.Text == "")
            {
                start = 0;
            }
            else if (!int.TryParse(txtUKStart.Text, out start))
            {
                MessageBox.Show("Start is a number only field");
                return;
            }
            if (txtUKEnd.Text == "")
            {
                end = 0;
            }
            else if (!int.TryParse(txtUKEnd.Text, out end))
            {
                MessageBox.Show("End is a number only field");
                return;
            }

            if (start > 0 && end > 0)
            {
                //Clear existing list

                //Get playlist tracks
                var playlist = spotify.GetPlaylist(MYSPOTIFY_NAME, CHARTS_PLAYLIST);

                Paging<PlaylistTrack> deleteTracks = spotify.GetPlaylistTracks(MYSPOTIFY_NAME, CHARTS_PLAYLIST);
                List<DeleteTrackUri> deleteList = new List<DeleteTrackUri>();

                foreach (PlaylistTrack t in deleteTracks.Items)
                {
                    deleteList.Add(new DeleteTrackUri(t.Track.Uri));
                }
                ErrorResponse doDelete = spotify.RemovePlaylistTracks(MYSPOTIFY_NAME, CHARTS_PLAYLIST, deleteList);

                //Add new tracks
                OfficialChartsEntities db = new OfficialChartsEntities();
                var tracksToRecord = from t in db.Record_Spotify
                                 //where t.TrackTitle.Contains("have you ever been lonely")
                             orderby t.ID
                             select t;
                List<Record_Spotify> lTracks = tracksToRecord.ToList();
                int perCycle = 100;
                int cycles = (end - start+1) / perCycle;

                for (int cycle=0; cycle < cycles; cycle++)
                {
                    string uris = "{\"uris\":[";
                    for (int t=0; t<perCycle;t++)
                    {
                        int trackNo = start + cycle * perCycle;
                        if (uris != "{\"uris\":[")
                        {
                            uris += ",";
                        }
                        string id = MySpotify.IdFromUrl(lTracks[t].SpotifyURL);
                        uris += ("\"spotify:track:" + id + "\"");
                    }
                    uris += "]}";
                    string url = "https://api.spotify.com/v1/playlists/2ps7zHd9of81uPSCNZxWEv/tracks" ;


                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                    myReq.Headers["Authorization"] = "Bearer " + spotify.AccessToken;
                    myReq.ContentType= "application/json";
                    myReq.Method = "POST";
                    using (var streamWriter = new StreamWriter(myReq.GetRequestStream()))
                    {
                        streamWriter.Write(uris);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    WebResponse wr = myReq.GetResponse();
                    wr.Dispose();
                }










                //ErrorResponse addTracks = spotify.AddPlaylistTracks(mySpotifyName, "2ps7zHd9of81uPSCNZxWEv", tracksToAdd);
                //if (addTracks.HasError())
                //{
                //    string debug = addTracks.Error.Message;
                //}

                GetPlayLists();
            }
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            CheckToken();
        }
    }
}
