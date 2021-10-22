using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class YT_Reports_Ent_2020
    {
        public string Month { get; set; }
        public string Year { get; set; }
       
        public string Asset_ID { get; set; }
        public string Asset_Title { get; set; }
        public string Asset_Labels { get; set; }
        public string Asset_Channel_ID { get; set; }
        public string Asset_Type { get; set; }
        public string Custom_ID { get; set; }        

        public int Owned_Views { get; set; }

        public decimal Partner_Revenue { get; set; }
        public decimal userShare { get; set; }

        public bool isRP { get; set; }


        public string UserId { get; set; }

        public int Id { get; set; }
        //public virtual YoutubeChannel YoutubeChannel { get; set; }
        public virtual YoutubeChannelsENT YoutubeChannelsENT { get; set; }
    }
}