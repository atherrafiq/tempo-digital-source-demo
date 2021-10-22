namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChannnelIDtoChannelID : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.YoutubeChannels", "ChannelID", c => c.String());
            DropColumn("dbo.YoutubeChannels", "ChannnelID");
        }
        
        public override void Down()
        {
            return;
            AddColumn("dbo.YoutubeChannels", "ChannnelID", c => c.String());
            DropColumn("dbo.YoutubeChannels", "ChannelID");
        }
    }
}
