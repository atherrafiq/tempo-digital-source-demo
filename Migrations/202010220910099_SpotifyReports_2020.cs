namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpotifyReports_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpotifyReports_2020",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        Country = c.String(),
                        Label = c.String(),
                        Product = c.String(),
                        URI = c.String(),
                        UPC = c.String(),
                        ISRC = c.String(),
                        TrackName = c.String(),
                        Artist_Name = c.String(),
                        Composer_Name = c.String(),
                        Album_Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        USD_Payable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SpotifyReports_2020");
        }
    }
}
