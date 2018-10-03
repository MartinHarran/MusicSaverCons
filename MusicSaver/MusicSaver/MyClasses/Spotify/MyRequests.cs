using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

using System.Web;
using MusicSaver.MyClasses;
using MusicSaver.MyClasses.Spotify;
using SpotifyAPI.Web.Models; //Models for the JSON-responses
using System.IO;
using static MusicSaver.MyClasses.Globals;

namespace MusicSaver.MyClasses.Spotify
{
    public class MyRequests
    {
        public static FullTrack GetTrack(string trackID)
        {
            FullTrack track = new FullTrack();
            string url = @"https://api.spotify.com/v1/tracks/" + trackID;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Method = "GET";
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            myReq.Headers["Authorization"] = "Bearer " + SPOTIFY_INST.AccessToken;
            try
            {
                WebResponse wr = myReq.GetResponse();
            }
            catch(WebException ex)
            {
                string debug = "stop";
                string exMessage = ex.Message;

                if (ex.Response != null)
                {
                    using (StreamReader responseReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        exMessage = responseReader.ReadToEnd();
                        errors.Add(exMessage);
                    }
                }
            }

            return track;
        }

        
    }
}
