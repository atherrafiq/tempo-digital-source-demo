using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class AmazonStreaming
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }
        public decimal UserShare { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public string Source { get; set; }
        public string Sales_Date { get; set; }
        public string Transaction_Date { get; set; }
        public string Territory { get; set; }
        public string UPC { get; set; }
        public string Release_Title { get; set; }
        public string Release_Artist { get; set; }
        public string Release_Label { get; set; }
        public string ISRC { get; set; }
        public string Track_Title { get; set; }
        public string Track_Artist { get; set; }
        public string Configuration { get; set; }
        public int Units { get; set; }
        public decimal Net_Amount { get; set; }

        public string test { get; set; }

    }
}