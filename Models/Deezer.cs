using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class Deezer
    {
        public Int64 DeezerID { get; set; }
        public int MusicReleaseId { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        public string ISRC { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Service { get; set; }
        public string Album { get; set; }
        public string UPC { get; set; }
        public string Country { get; set; }
        public int Nb_of_plays { get; set; }
        public decimal Royalties { get; set; }
        public string Provider { get; set; }
        public string Provider_id { get; set; }
        public string Label { get; set; }
        public decimal Share { get; set; }

        //[ForeignKey("MusicReleaseId")]
        //public virtual MusicRelease MusicRelease { get; set; }
    }
}