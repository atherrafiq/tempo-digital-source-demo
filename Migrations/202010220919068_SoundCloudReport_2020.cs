namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoundCloudReport_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoundCloudReport_2020",
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
                        Share = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SoundCloudReport_2020");
        }
    }
}
