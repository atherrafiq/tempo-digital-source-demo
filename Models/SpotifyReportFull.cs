﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class SpotifyReportFull
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }

        public string Country { get; set; }
        public string Label { get; set; }
        public string Product { get; set; }
        public string URI { get; set; }
        public string UPC { get; set; }
        public string ISRC { get; set; }
        public string TrackName { get; set; }
        public string Artist_Name { get; set; }
        public string Composer_Name { get; set; }
        public string Album_Name { get; set; }
        public int Quantity { get; set; }
        public decimal USD_Payable { get; set; }
        public decimal share { get; set; }

        //[ForeignKey("MusicReleaseId")]
        //public virtual MusicRelease MusicRelease { get; set; }
    }
}