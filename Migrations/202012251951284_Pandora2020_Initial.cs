namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pandora2020_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PandoraReports_2020",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        DSP = c.String(),
                        SalesDate = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        ReleaseTitle = c.String(),
                        ResourceTitle = c.String(),
                        Contributors = c.String(),
                        RoyaltyRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfConsumerSalesGross = c.Int(nullable: false),
                        TerritoryCode = c.String(),
                        PlanName = c.String(),
                        LabelName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PandoraReports_2020");
        }
    }
}
