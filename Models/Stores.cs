using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class Stores
    {
        [Key]
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public bool isSelected { get; set; }
        public string iTunesReleasePriceTier { get; set; }
        public string iTunesTrackPriceTier { get; set; }
        public bool updateToNewStores { get; set; }
        public bool isYoutubeSelected { get; set; }
        public bool isFacebookSelected { get; set; }


        public int MusicReleaseId { get; set; }

        [ForeignKey("MusicReleaseId")]
        public virtual MusicRelease MusicRelease { get; set; }
    }
}