namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewRelationship : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.YT_Reports_Entertainment_Full", "YoutubeChannelsENT_ID", c => c.Int());
            CreateIndex("dbo.YT_Reports_Entertainment_Full", "YoutubeChannelsENT_ID");
            AddForeignKey("dbo.YT_Reports_Entertainment_Full", "YoutubeChannelsENT_ID", "dbo.YoutubeChannelsENTs", "ID");
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.YT_Reports_Entertainment_Full", "YoutubeChannelsENT_ID", "dbo.YoutubeChannelsENTs");
            DropIndex("dbo.YT_Reports_Entertainment_Full", new[] { "YoutubeChannelsENT_ID" });
            DropColumn("dbo.YT_Reports_Entertainment_Full", "YoutubeChannelsENT_ID");
        }
    }
}
