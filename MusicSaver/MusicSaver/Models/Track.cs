//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicSaver.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Track
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Track()
        {
            this.Charts = new HashSet<Chart>();
            this.ChartBillboard100 = new HashSet<ChartBillboard100>();
            this.OnDiscs = new HashSet<OnDisc>();
        }
    
        public string TrackID { get; set; }
        public string TrackTitle { get; set; }
        public string ArtistID { get; set; }
        public string ArtistName { get; set; }
        public Nullable<int> MMID { get; set; }
        public string PathOnDisc { get; set; }
        public string Spotify { get; set; }
        public string Deezer { get; set; }
        public string Amazon { get; set; }
        public string iTunes { get; set; }
        public string GooglePlay { get; set; }
        public string AdjTrackTitle { get; set; }
        public string AdjArtistName { get; set; }
    
        public virtual Artist Artist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chart> Charts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChartBillboard100> ChartBillboard100 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnDisc> OnDiscs { get; set; }
    }
}
