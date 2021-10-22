namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YT_RP_Report_Music_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YT_RP_Report_Music_2020",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        Asset_ID = c.String(),
                        Video_ID = c.String(),
                        Asset_Title = c.String(),
                        Counrty = c.String(),
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
                        Owned_Views = c.Int(nullable: false),
                        Partner_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        userShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.YoutubeChannels", t => t.YoutubeChannel_ID)
                .Index(t => t.YoutubeChannel_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YT_RP_Report_Music_2020", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.YT_RP_Report_Music_2020", new[] { "YoutubeChannel_ID" });
            DropTable("dbo.YT_RP_Report_Music_2020");
        }
    }
}
