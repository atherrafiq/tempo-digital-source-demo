namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeezerReport_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeezerReport_2020",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        ISRC = c.String(),
                        Artist = c.String(),
                        Title = c.String(),
                        Service = c.String(),
                        Album = c.String(),
                        UPC = c.String(),
                        Country = c.String(),
                        Nb_of_plays = c.Int(nullable: false),
                        Royalties = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Provider = c.String(),
                        Provider_id = c.String(),
                        Label = c.String(),
                        Share = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeezerReport_2020");
        }
    }
}
