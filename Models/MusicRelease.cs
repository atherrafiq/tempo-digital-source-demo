using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class MusicRelease
    {
        public int MusicReleaseId { get; set; }
        
        public bool isAlbum { get; set; }

        public string ReleaseName { get; set; }
        public string MainArtist { get; set; }
        public string FeaturedArtist { get; set; }
        public string Language { get; set; }
        public string PrimaryGenre { get; set; }
        public string SecondaryGenre { get; set; }
        public string CopyrightYear { get; set; }
        public string CopyryghtHolder { get; set; }
        public string LabelName { get; set; }
        public string UPCEAN { get; set; }
        public string RecordingLocation { get; set; }
        public string countries { get; set; }
        public string CoverImage { get; set; }
        public DateTime Sales_Start_Date { get; set; }
        public bool isPreviouslyReleased { get; set; }
        public string UserId { get; set; }
        public string Publisher { get; set; }
        public string Linkfire_url { get; set; }

        public bool needUPC { get; set; }
        public bool wantYoutube { get; set; }
        public DateTime currentDateTime { get; set; }
        public bool isVA { get; set; }
        public string Status { get; set; }
        public string DeletionDate { get; set; }
        public string PaymentStaus { get; set; }

        public string ReleaseFormat { get; set; }
        public string ReleaseVersion { get; set; }
        public string ReleaseCategory { get; set; }
        public string ReleaseDate { get; set; }
        public string OriginalReleaseDate { get; set; }
        public string MetadataLanguage { get; set; }
        public string ReleaseAdvisory { get; set; }
        public string ImageFormat { get; set; }
        public string TerritoryAvailability { get; set; }
        public string CInfo { get; set; }
        public string CYear { get; set; }
        public string PInfo { get; set; }
        public string PYear { get; set; }
        public string Update_Status { get; set; }
        public string Itune_Track_Price { get; set; }
        public string Itune_Release_Price { get; set; }

        public bool artistToAllSongs { get; set; }
        public bool copyRightToAllSongs { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual List<Artist> Artists { get; set; }
        public virtual List<FeaturedArtist> FeaturedArtists { get; set; }
        public virtual List<Song> Songs { get; set; }
        public virtual List<Stores> Stores { get; set; }
        public virtual List<Participants> Participants { get; set; }
    }
}