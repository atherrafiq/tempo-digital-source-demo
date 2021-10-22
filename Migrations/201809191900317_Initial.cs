namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(),
                        MusicReleaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArtistId)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
            CreateTable(
                "dbo.MusicReleases",
                c => new
                    {
                        MusicReleaseId = c.Int(nullable: false, identity: true),
                        ReleaseName = c.String(),
                        MainArtist = c.String(),
                        FeaturedArtist = c.String(),
                        Language = c.String(),
                        PrimaryGenre = c.String(),
                        SecondaryGenre = c.String(),
                        CopyrightYear = c.String(),
                        CopyryghtHolder = c.String(),
                        LabelName = c.String(),
                        UPCEAN = c.String(),
                        ISRC = c.String(),
                        RecordingLocation = c.String(),
                    })
                .PrimaryKey(t => t.MusicReleaseId);
            
            CreateTable(
                "dbo.FeaturedArtists",
                c => new
                    {
                        FeaturedArtistId = c.Int(nullable: false, identity: true),
                        FeaturedArtistName = c.String(),
                        MusicReleaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeaturedArtistId)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongId = c.Int(nullable: false, identity: true),
                        SongTitle = c.String(),
                        SongDuration = c.Time(nullable: false, precision: 7),
                        MusicReleiseId = c.Int(nullable: false),
                        MusicRelease_MusicReleaseId = c.Int(),
                    })
                .PrimaryKey(t => t.SongId)
                .ForeignKey("dbo.MusicReleases", t => t.MusicRelease_MusicReleaseId)
                .Index(t => t.MusicRelease_MusicReleaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "MusicRelease_MusicReleaseId", "dbo.MusicReleases");
            DropForeignKey("dbo.FeaturedArtists", "MusicReleaseId", "dbo.MusicReleases");
            DropForeignKey("dbo.Artists", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Songs", new[] { "MusicRelease_MusicReleaseId" });
            DropIndex("dbo.FeaturedArtists", new[] { "MusicReleaseId" });
            DropIndex("dbo.Artists", new[] { "MusicReleaseId" });
            DropTable("dbo.Songs");
            DropTable("dbo.FeaturedArtists");
            DropTable("dbo.MusicReleases");
            DropTable("dbo.Artists");
        }
    }
}
