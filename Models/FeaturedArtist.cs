using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class FeaturedArtist
    {

        public int FeaturedArtistId { get; set; }

        public string FeaturedArtistName { get; set; }


        public int MusicReleaseId { get; set; }
        public virtual MusicRelease MusicRelease { get; set; }
    }
}