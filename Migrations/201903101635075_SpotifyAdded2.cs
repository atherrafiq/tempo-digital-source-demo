namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpotifyAdded2 : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.SpotifyReports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        LabelName = c.String(),
                        Month = c.String(),
                        Year = c.String(),
                        Quantity = c.Int(nullable: false),
                        Payable = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.SpotifyReports", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.SpotifyReports", new[] { "MusicReleaseId" });
            DropTable("dbo.SpotifyReports");
        }
    }
}
