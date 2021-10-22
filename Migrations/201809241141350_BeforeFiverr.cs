namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeforeFiverr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "countries", c => c.String());
            AddColumn("dbo.MusicReleases", "Sales_Start_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.MusicReleases", "isPreviouslyReleased", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "isrc", c => c.Int(nullable: false));
            AddColumn("dbo.Songs", "version", c => c.String());
            AddColumn("dbo.Songs", "isExplicit_Lyrics", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "isCleant_Lyrics", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "RecordingYear", c => c.String());
            DropColumn("dbo.MusicReleases", "ISRC");
            DropColumn("dbo.MusicReleases", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MusicReleases", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.MusicReleases", "ISRC", c => c.String());
            DropColumn("dbo.Songs", "RecordingYear");
            DropColumn("dbo.Songs", "isCleant_Lyrics");
            DropColumn("dbo.Songs", "isExplicit_Lyrics");
            DropColumn("dbo.Songs", "version");
            DropColumn("dbo.Songs", "isrc");
            DropColumn("dbo.MusicReleases", "isPreviouslyReleased");
            DropColumn("dbo.MusicReleases", "Sales_Start_Date");
            DropColumn("dbo.MusicReleases", "countries");
        }
    }
}
