namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppleMusic2_Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apple_Music",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        Contract_Name = c.String(),
                        Type = c.String(),
                        Sales_Date = c.String(),
                        Transaction_Date = c.String(),
                        Source = c.String(),
                        Sub_Source = c.String(),
                        Territory = c.String(),
                        Barcode = c.String(),
                        Cat_No = c.String(),
                        Release_Title = c.String(),
                        Release_Artist = c.String(),
                        ISRC = c.String(),
                        Track_Title = c.String(),
                        Track_Artist = c.String(),
                        Distribution_Channel = c.String(),
                        Configuration = c.String(),
                        Units = c.Int(nullable: false),
                        Original_Currency = c.String(),
                        Net_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Apple_Music");
        }
    }
}
