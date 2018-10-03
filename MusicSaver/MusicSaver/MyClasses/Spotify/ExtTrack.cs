using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyAPI.Web.Models; //Models for the JSON-responses
using AutoMapper;
using System.Text.RegularExpressions;

namespace MusicSaver.MyClasses.Spotify
{
    class ExtTrack : FullTrack
    {
        public List<string> Uris { get; set; }
        public string Filename { get; set; }
        public string OCID;
        public string OCName;
        public string OCArtist;
        public string[] TagArtists;
        public string[] TagGenres;
        public Nullable<int> Year;

        public ExtTrack() { }

        public void AddProps(string name, string artist, string ocid)
        {
            name = name.Trim();
            artist = artist.Trim();
            OCID = ocid;
            OCName = Regex.Replace(name, "the ", "", RegexOptions.IgnoreCase);
            OCArtist = Regex.Replace(artist, "the ", "", RegexOptions.IgnoreCase);
            Year = null;
            Uris = new List<string>();
            if (Uri!=null) Uris.Add("{\"" + Uri.ToString() + "\"}");
            Filename = OCName +'_' +OCArtist;
            Filename = Filename.Replace("\"", "'");
            Filename = Filename.Replace("\\", "/");
            if (Artists.Count > 0)
            {
                Name += ("_" + Artists[0]);
                List<string> artists = new List<string>();
                foreach (SimpleArtist a in Artists)
                {
                    artists.Add(a.Name);
                }
                TagArtists = artists.ToArray();
            }

            List<string> genres = new List<string>();

        }
    }
}
