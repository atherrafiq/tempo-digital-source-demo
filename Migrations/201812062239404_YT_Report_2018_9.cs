namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YT_Report_2018_9 : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.YT_Report_2018_9",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportID = c.Long(nullable: false),
                        Month = c.String(),
                        Year = c.String(),
                        ChannelName = c.String(),
                        ChannelID = c.String(),
                        AssetTitle = c.String(),
                        AssetType = c.String(),
                        Views = c.Int(nullable: false),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropForeignKey("dbo.YT_Report_2018_9", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.YT_Report_2018_9", new[] { "YoutubeChannel_ID" });
            DropTable("dbo.YT_Report_2018_9");
        }
    }
}
