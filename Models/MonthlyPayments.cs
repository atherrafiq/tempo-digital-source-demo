using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class MonthlyPayments
    {
        public int ID { get; set; }
        public string User { get; set; }
        public string MusicAmount { get; set; }
        public string EntAmount { get; set; }
        public string Date_Time { get; set; }
        public string Music_Month { get; set; }
        public string Music_Year { get; set; }
        public string Ent_Month { get; set; }
        public string Ent_Year { get; set; }
        public bool Is_Paid { get; set; }
    }
}