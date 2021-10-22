using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class PandoraReports_2020
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }
        public decimal share { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string DSP { get; set; }
        public string SalesDate { get; set; }
        public string ISRC { get; set; }
        public string UPC { get; set; }
        public string ReleaseTitle { get; set; }
        public string ResourceTitle { get; set; }
        public string Contributors { get; set; }
        public decimal RoyaltyRate { get; set; }
        public int NumberOfConsumerSalesGross { get; set; }
        public string TerritoryCode { get; set; }
        public string PlanName { get; set; }
        public string LabelName { get; set; }
    }
}