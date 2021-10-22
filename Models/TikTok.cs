using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class TikTok
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }
        public decimal share { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public string report_start_date { get; set; }
        public string report_end_date { get; set; }
        public string platform_name { get; set; }
        public string content_provider { get; set; }
        public string platform_song_id { get; set; }
        public string song_title { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public string label_name { get; set; }
        public string isrc { get; set; }
        public string product_code { get; set; }
        public string territory { get; set; }
        public int video_creations { get; set; }
        public int vv { get; set; }
        public decimal statement_amount { get; set; }
        public string statement_currency { get; set; }
        public string genre { get; set; }

    }
}