using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class UserLogs
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string ActionPerformed { get; set; }
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ClientIP { get; set; }
        public string Other { get; set; }
    }

    
}