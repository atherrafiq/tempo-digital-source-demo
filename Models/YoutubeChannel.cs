using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class YoutubeChannel
    {
        public int ID{ get; set; }
        public string ChannelName { get; set; }
        public string ChannelID { get; set; }
        public string Description { get; set; }
        public string YT_Label_Name { get; set; }
        public string YT_Label_Description { get; set; }
        public bool Approved { get; set; }
        public string ApprovelDate { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

       
       // public virtual List<YT_Report_2018_8> YT_Report_2018_8 { get; set; }

    }
}