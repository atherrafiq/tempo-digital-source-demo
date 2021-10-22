using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TempoDigitalApex3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string LabelName { get; internal set; }
        public string AtristName { get; internal set; }
        public string share { get; internal set; }
        public string Bank { get; internal set; }
        public string Paypal { get; internal set; }
        public string BankName { get; internal set; }
        public string BankAddress { get; internal set; }
        public string BankIBAN { get; internal set; }
        public string BankCountry { get; internal set; }
        public string BankEmail { get; internal set; }
        public string CompanyName { get; internal set; }
        public string CompanyAddress { get; internal set; }
        public string Contact { get; internal set; }
        public string UserType { get; internal set; }
        public bool isActive { get; internal set; }
        public bool isYoutube { get; internal set; }
        public bool isDistribution { get; internal set; }
        public bool hasContract { get; internal set; }
        public bool isCustomUser { get; internal set; }
        public string whoManagedUser { get; internal set; }
        public DateTime DateTimeCreated { get; internal set; } = DateTime.Now;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims hsssere
            userIdentity.AddClaim(new Claim("FirstName", this.FirstName));
            userIdentity.AddClaim(new Claim("LastName", this.LastName));
            userIdentity.AddClaim(new Claim("LabelName", this.LabelName));
            userIdentity.AddClaim(new Claim("AtristName", this.AtristName));
            userIdentity.AddClaim(new Claim("Share", this.share));
            userIdentity.AddClaim(new Claim("Bank", this.share));
            userIdentity.AddClaim(new Claim("Paypal", this.share));
            userIdentity.AddClaim(new Claim("BankName", this.share));
            userIdentity.AddClaim(new Claim("BankAddress", this.share));
            userIdentity.AddClaim(new Claim("BankIBAN", this.share));
            userIdentity.AddClaim(new Claim("BankCountry", this.share));
            userIdentity.AddClaim(new Claim("BankEmail", this.share));
            userIdentity.AddClaim(new Claim("CompanyName", this.share));
            userIdentity.AddClaim(new Claim("CompanyAddress", this.share));
            userIdentity.AddClaim(new Claim("Contact", this.share));
            userIdentity.AddClaim(new Claim("UserType", this.share));
            userIdentity.AddClaim(new Claim("isActive", this.share));
            userIdentity.AddClaim(new Claim("isYoutube", this.share));
            userIdentity.AddClaim(new Claim("isDistribution", this.share));
            userIdentity.AddClaim(new Claim("hasContract", this.share));
            userIdentity.AddClaim(new Claim("isCustomUser", this.share));
            userIdentity.AddClaim(new Claim("whoManagedUser", this.share));
            userIdentity.AddClaim(new Claim("DateTimeCreated", this.share));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<MusicRelease> MusicReleases { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<FeaturedArtist> FeaturedArtists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Stores> Stores { get; set; }
        public DbSet<YoutubeChannel> YoutubeChannels { get; set; }
        public DbSet<YoutubeChannelsENT> GetYoutubeChannelsENT { get; set; }
        /////public DbSet<YT_Report_2018_8> YT_Report_2018_8 { get; set; }
        public DbSet<YT_Report_2018_9> YT_Report_2018_9 { get; set; }
        public DbSet<Tidal> Tidal { get; set; }
        public DbSet<TikTok> TikTok { get; set; }
        public DbSet<Pandora> Pandora { get; set; }
        public DbSet<Apple_Music> Apple_Music { get; set; }
        public DbSet<Apple_Music_New> Apple_Music_New { get; set; }
        public DbSet<Napster_Streaming> Napster_streaming { get; set; }
        public DbSet<UPC_Codes> UPC_Codes { get; set; }
        public DbSet<ISRC_Codes> ISRC_Codes { get; set; }

        public DbSet<YT_Reports_Music_2020> YT_Reports_Music_2020 { get; set; }
        public DbSet<YT_Reports_Ent_2020> YT_Reports_Ent_2020 { get; set; }
        public DbSet<YT_RP_Report_Music_2020> YT_RP_Report_Music_2020 { get; set; }
        public DbSet<YT_RP_Report_Ent_2020> YT_RP_Report_Ent_2020 { get; set; }
        public DbSet<YT_ART_TRACK_Report_2020> YT_ART_TRACK_Report_2020 { get; set; }
        public DbSet<SpotifyReports_2020> SpotifyReports_2020 { get; set; }
        public DbSet<DeezerReport_2020> DeezerReport_2020 { get; set; }
        public DbSet<GoogleMusicReport_2020> GoogleMusicReport_2020 { get; set; }
        public DbSet<SoundCloudReport_2020> SoundCloudReport_2020 { get; set; }
        public DbSet<PandoraReports_2020> PandoraReports_2020 { get; set; }


        public DbSet<Participants> Participants { get; set; }
        public DbSet<SongParticipent> SongParticipent { get; set; }

        //public DbSet<YT_Reports> YT_Reports { get; set; }
        public DbSet<SpotifyReport> SpotifyReports { get; set; }
        public DbSet<Deezer> Deezers { get; set; }
        public DbSet<Google_Play> Google_Play { get; set; }
        public DbSet<SoundCloud> SoundCloud { get; set; }
        public DbSet<SpotifyReportFull> SpotifyReportFull { get; set; }
        public DbSet<YT_Reports_Music_Full> YT_Reports_Music_Full { get; set; }
        public DbSet<YT_Reports_Entertainment_Full> YT_Reports_Entertainment_Full_new { get; set; }
        public DbSet<YT_Reports_Entertainment_Full_New> YT_Reports_Entertainment_Full_New { get; set; }

        public DbSet<YT_Reports_Ent> yT_Reports_Ents { get; set; }

        public DbSet<UPC_ISRC> UPC_ISRCs { get; set; }

        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<UserLogs> UserLogs { get; set; }
        public DbSet<MonthlyPayments> MonthlyPayments { get; set; }
        public DbSet<UserFiles> UserFiles { get; set; }

        public DbSet<YoutubeConnect> YoutubeConnect { get; set; }

        public DbSet<mail_templates> mail_Templates { get; set; }


        // public DbSet<AspNetUsers> AspNetUsers { get;set }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}