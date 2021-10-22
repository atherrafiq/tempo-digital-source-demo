namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeezerReportsAdded : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.Deezers",
                c => new
                    {
                        DeezerID = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        ISRC = c.String(),
                        Artist = c.String(),
                        Album = c.String(),
                        UPC = c.String(),
                        Country = c.String(),
                        Nb_of_plays = c.Int(nullable: false),
                        Royalties = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Provider = c.String(),
                        Provider_id = c.String(),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.DeezerID)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.Deezers", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Deezers", new[] { "MusicReleaseId" });
            DropTable("dbo.Deezers");
        }
    }
}
