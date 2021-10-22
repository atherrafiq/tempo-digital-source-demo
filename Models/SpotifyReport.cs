using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class SpotifyReport
    {
        public Int64 Id { get; set; }
        public int MusicReleaseId { get; set; }
        public string LabelName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int Quantity { get; set; }
        public decimal Payable { get; set; }

        [ForeignKey("MusicReleaseId")]
        public virtual MusicRelease MusicRelease { get; set; }
    }
}