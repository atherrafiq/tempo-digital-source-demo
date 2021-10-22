namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedReleaseidfromspotify : DbMigration
    {
        public override void Up()
        {
            return;
            DropForeignKey("dbo.SpotifyReportFulls", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.SpotifyReportFulls", new[] { "MusicReleaseId" });
        }
        
        public override void Down()
        {
            return;
            CreateIndex("dbo.SpotifyReportFulls", "MusicReleaseId");
            AddForeignKey("dbo.SpotifyReportFulls", "MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId", cascadeDelete: true);
        }
    }
}
