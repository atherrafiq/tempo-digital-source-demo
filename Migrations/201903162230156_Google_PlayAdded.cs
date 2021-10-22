namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Google_PlayAdded : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.Google_Play",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        UPC = c.String(),
                        GRID = c.String(),
                        ISRC = c.String(),
                        Artist = c.String(),
                        Product_Title = c.String(),
                        Container_Title = c.String(),
                        Content_Provider = c.String(),
                        Label = c.String(),
                        Consumer_Country = c.String(),
                        Device_Type = c.String(),
                        Product_Type = c.String(),
                        Interaction_Type = c.String(),
                        Count = c.Int(nullable: false),
                        Total_Plays = c.Int(nullable: false),
                        Partner_Revenue_Paid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Currency = c.String(),
                        EUR_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.Google_Play", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Google_Play", new[] { "MusicReleaseId" });
            DropTable("dbo.Google_Play");
        }
    }
}
