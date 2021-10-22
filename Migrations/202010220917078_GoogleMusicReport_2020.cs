namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoogleMusicReport_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoogleMusicReport_2020",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(),
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
                        Share = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GoogleMusicReport_2020");
        }
    }
}
