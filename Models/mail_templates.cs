using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class mail_templates
    {
        public Int64 Id { get; set; }
        public string template { get; set; }
        public string other { get; set; }
        public string type { get; set; }
    }
}