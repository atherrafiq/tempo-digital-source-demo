using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class SongParticipent
    {
        [Key]
        public int ParticipantID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantRole { get; set; }
        public string RoleType { get; set; }
        public string Instrument { get; set; }
        public string IPICode { get; set; }
        public string IPNCode { get; set; }
        public bool? isPrimary { get; set; }
        public bool? needIPI { get; set; }
        public bool? needIPN { get; set; }
        public bool? needYoCopyToAllTracks { get; set; }

        public int SongId { get; set; }

        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }
    }
}