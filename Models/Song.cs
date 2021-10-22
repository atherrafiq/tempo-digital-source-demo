using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string SongTitle { get; set; }
        public TimeSpan SongDuration { get; set; }
        public string isrc { get; set; }
        public string version { get; set; }
        public bool isExplicit_Lyrics { get; set; }
        public bool isCleant_Lyrics { get; set; }
        public string RecordingYear { get; set; }
        public string SongPath { get; set; }
        public int MusicReleaseId { get; set; }
        public bool needISRC { get; set; }
        public string Length { get; set; }
        public bool isVAR { get; set; }
        public string ArtistName { get; set; }
        public string ComposerName { get; set; }
        public string WriterName { get; set; }
        public string ArrangerName { get; set; }

        public string CInfo { get; set; }
        public string CYear { get; set; }
        public string PInfo { get; set; }
        public string PYear { get; set; }
        public string ISWC { get; set; }
        public string Advisory { get; set; }
        public string CopyRightHolder { get; set; }
        public bool PermitDownload { get; set; }
        public bool PermitStreaming { get; set; }
        public DateTime? DownloadDate { get; set; }
        public DateTime? StreamingDate { get; set; }
        

        [ForeignKey("MusicReleaseId")]
        public virtual MusicRelease MusicRelease { get; set; }

        public virtual List<SongParticipent> SongParticipent { get; set; }

    }
}