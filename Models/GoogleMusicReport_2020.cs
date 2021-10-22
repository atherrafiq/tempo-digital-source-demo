using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class GoogleMusicReport_2020
    {
        public Int64 Id { get; set; }
        public int? MusicReleaseId { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        public string UPC { get; set; }
        public string GRID { get; set; }
        public string ISRC { get; set; }
        public string Artist { get; set; }
        public string Product_Title { get; set; }   //Song Name
        public string Container_Title { get; set; }   //Release Name
        public string Content_Provider { get; set; }
        public string Label { get; set; }
        public string Consumer_Country { get; set; }
        public string Device_Type { get; set; }
        public string Product_Type { get; set; }
        public string Interaction_Type { get; set; }
        public int Count { get; set; }
        public int Total_Plays { get; set; }
        public decimal Partner_Revenue_Paid { get; set; }
        public string Partner_Revenue_Currency { get; set; }
        public decimal EUR_Amount { get; set; }
        public decimal Share { get; set; }
    }
}