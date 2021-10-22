using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class UPC_ISRC
    {
        public int ID { get; set; }
        public string UPC { get; set; }
        public string UPC_status { get; set; }
        public string ISRC { get; set; }
        public string ISRC_status { get; set; }
    }
}