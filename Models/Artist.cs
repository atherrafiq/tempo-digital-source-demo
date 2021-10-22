using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        
        public string ArtistName { get; set; }


        public int MusicReleaseId { get; set; }
        public virtual MusicRelease MusicRelease { get; set; }

    }
}