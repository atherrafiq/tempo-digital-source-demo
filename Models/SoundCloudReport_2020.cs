using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class SoundCloudReport_2020
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }

        public string Month { get; set; }
        public string Year { get; set; }
        public string Partner_Name { get; set; }
        public string LabelName { get; set; }
        public string Account_ID { get; set; }
        public string Account_Name { get; set; }
        public string Artist_Name { get; set; }
        public string Album_Title { get; set; }
        public string Track_Name { get; set; }
        public string Track_ID { get; set; }
        public string Track_Classification { get; set; }
        public string ISRC { get; set; }
        public string UPC { get; set; }
        public string Territory { get; set; }
        public int Total_Plays { get; set; }
        public decimal Total_Revenue { get; set; }
        public string Revenue_Currency { get; set; }
        public string Monetisation_Type { get; set; }
        public string Usage_Type { get; set; }
        public int Version { get; set; }
        public decimal Share { get; set; }
    }
}