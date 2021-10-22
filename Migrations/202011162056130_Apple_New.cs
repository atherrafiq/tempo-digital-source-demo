namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Apple_New : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apple_Music_New",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        UserShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        Sales_Date = c.String(),
                        Transaction_Date = c.String(),
                        Territory = c.String(),
                        UPC = c.String(),
                        Release_Title = c.String(),
                        Release_Artist = c.String(),
                        ISRC = c.String(),
                        Track_Title = c.String(),
                        Track_Artist = c.String(),
                        Configuration = c.String(),
                        Units = c.Int(nullable: false),
                        Net_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Apple_Music_New");
        }
    }
}
