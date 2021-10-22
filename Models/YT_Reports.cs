using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class YT_Reports
    {
        
        public string Month { get; set; }
        public string Year { get; set; }
        public string ChannelName { get; set; }
        public string ChannelID { get; set; }
        public string AssetTitle { get; set; }
        public string AssetType { get; set; }
        public int Views { get; set; }
        public decimal Revenue { get; set; }

        public string UserId { get; set; }

        public int Id { get; set; }
        public virtual YoutubeChannel YoutubeChannel { get; set; }
    }
}