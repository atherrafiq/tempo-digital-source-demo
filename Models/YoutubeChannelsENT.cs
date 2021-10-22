using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class YoutubeChannelsENT
    {
        public int ID { get; set; }
        public string ChannelName { get; set; }
        public string ChannelID { get; set; }
        public string Description { get; set; }
        public string YT_Label_Name { get; set; }
        public string YT_Label_Description { get; set; }
        public bool Approved { get; set; }
        public string ApprovelDate { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}