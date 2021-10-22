namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YT_RP_Report_Ent_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YT_RP_Report_Ent_2020",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        Asset_ID = c.String(),
                        Video_Title = c.String(),
                        Contry = c.String(),
                        Video_ID = c.String(),
                        Content_Type = c.String(),
                        Asset_Labels = c.String(),
                        Asset_Channel_ID = c.String(),
                        Asset_Type = c.String(),
                        Custom_ID = c.String(),
                        Claim_Type = c.String(),
                        Owned_Views = c.Int(nullable: false),
                        Partner_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        userShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannelsENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.YoutubeChannelsENTs", t => t.YoutubeChannelsENT_ID)
                .Index(t => t.YoutubeChannelsENT_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YT_RP_Report_Ent_2020", "YoutubeChannelsENT_ID", "dbo.YoutubeChannelsENTs");
            DropIndex("dbo.YT_RP_Report_Ent_2020", new[] { "YoutubeChannelsENT_ID" });
            DropTable("dbo.YT_RP_Report_Ent_2020");
        }
    }
}
