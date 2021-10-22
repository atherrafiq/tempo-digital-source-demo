namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChannelUrltoChannelID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YoutubeChannels", "ChannnelID", c => c.String());
            DropColumn("dbo.YoutubeChannels", "ChannnelUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YoutubeChannels", "ChannnelUrl", c => c.String());
            DropColumn("dbo.YoutubeChannels", "ChannnelID");
        }
    }
}
