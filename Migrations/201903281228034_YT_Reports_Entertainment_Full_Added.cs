namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YT_Reports_Entertainment_Full_Added : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.YT_Reports_Entertainment_Full",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        Adjustment_Type = c.String(),
                        Asset_ID = c.String(),
                        Asset_Title = c.String(),
                        Asset_Labels = c.String(),
                        Asset_Channel_ID = c.String(),
                        Asset_Type = c.String(),
                        Custom_ID = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        GRid = c.String(),
                        Artist = c.String(),
                        Album = c.String(),
                        Label = c.String(),
                        Administer_Publish_Right = c.String(),
                        Owned_Views = c.Int(nullable: false),
                        YouTube_Revenue_Split_Auction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split_Reserved = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split_Partner_Sold_Youtube_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Auction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Reserved = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Partner_Sold_Youtube_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Partner_Sold_Partner_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.YoutubeChannels", t => t.YoutubeChannel_ID)
                .Index(t => t.YoutubeChannel_ID);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.YT_Reports_Entertainment_Full", new[] { "YoutubeChannel_ID" });
            DropTable("dbo.YT_Reports_Entertainment_Full");
        }
    }
}
