using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class Pandora
    {

        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }
        public decimal share { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string testtest_migration { get; set; }

        public string Contract_Name { get; set; }
        public string Type { get; set; }
        public string Sales_Date { get; set; }
        public string Transaction_Date { get; set; }
        public string Source { get; set; }
        public string Sub_Source { get; set; }
        public string Territory { get; set; }
        public string Barcode { get; set; }
        public string Cat_No { get; set; }
        public string Release_Title { get; set; }
        public string Release_Artist { get; set; }
        public string ISRC { get; set; }
        public string Track_Title { get; set; }
        public string Track_Artist { get; set; }
        public string Distribution_Channel { get; set; }
        public string Configuration { get; set; }
        public int Units { get; set; }
        public string Original_Currency { get; set; }
        public decimal Net_Amount { get; set; }

    }
}