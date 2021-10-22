namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_relationships_from_reports_musicreleaseid : DbMigration
    {
        public override void Up()
        {
            return;
            DropForeignKey("dbo.Deezers", "MusicReleaseId", "dbo.MusicReleases");
            DropForeignKey("dbo.Google_Play", "MusicReleaseId", "dbo.MusicReleases");
            DropForeignKey("dbo.SoundClouds", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Deezers", new[] { "MusicReleaseId" });
            DropIndex("dbo.Google_Play", new[] { "MusicReleaseId" });
            DropIndex("dbo.SoundClouds", new[] { "MusicReleaseId" });
        }
        
        public override void Down()
        {
            return;
            CreateIndex("dbo.SoundClouds", "MusicReleaseId");
            CreateIndex("dbo.Google_Play", "MusicReleaseId");
            CreateIndex("dbo.Deezers", "MusicReleaseId");
            AddForeignKey("dbo.SoundClouds", "MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId", cascadeDelete: true);
            AddForeignKey("dbo.Google_Play", "MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId", cascadeDelete: true);
            AddForeignKey("dbo.Deezers", "MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId", cascadeDelete: true);
        }
    }
}
