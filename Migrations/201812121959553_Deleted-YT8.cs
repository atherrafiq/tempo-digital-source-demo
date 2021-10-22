namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedYT8 : DbMigration
    {
        public override void Up()
        {
            return;
            DropForeignKey("dbo.YT_Report_2018_8", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.YT_Report_2018_8", new[] { "YoutubeChannel_ID" });
            DropTable("dbo.YT_Report_2018_8");
        }
        
        public override void Down()
        {
            return;
            CreateTable(
                "dbo.YT_Report_2018_8",
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.YT_Report_2018_8", "YoutubeChannel_ID");
            AddForeignKey("dbo.YT_Report_2018_8", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
        }
    }
}
