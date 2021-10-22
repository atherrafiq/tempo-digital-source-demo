using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class UserFiles
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string DateTime { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public string Other { get; set; }
    }
}