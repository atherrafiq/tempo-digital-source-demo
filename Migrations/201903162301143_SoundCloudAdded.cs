namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoundCloudAdded : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.SoundClouds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        Partner_Name = c.String(),
                        LabelName = c.String(),
                        Account_ID = c.String(),
                        Account_Name = c.String(),
                        Artist_Name = c.String(),
                        Album_Title = c.String(),
                        Track_Name = c.String(),
                        Track_ID = c.String(),
                        Track_Classification = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        Territory = c.String(),
                        Total_Plays = c.Int(nullable: false),
                        Total_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Revenue_Currency = c.String(),
                        Monetisation_Type = c.String(),
                        Usage_Type = c.String(),
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.SoundClouds", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.SoundClouds", new[] { "MusicReleaseId" });
            DropTable("dbo.SoundClouds");
        }
    }
}
