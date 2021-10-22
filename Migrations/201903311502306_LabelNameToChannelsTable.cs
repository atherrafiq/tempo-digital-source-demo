namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabelNameToChannelsTable : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.YoutubeChannels", "YT_Label_Name", c => c.String());
            AddColumn("dbo.YoutubeChannels", "YT_Label_Description", c => c.String());
        }
        
        public override void Down()
        {
            return;
            DropColumn("dbo.YoutubeChannels", "YT_Label_Description");
            DropColumn("dbo.YoutubeChannels", "YT_Label_Name");
        }
    }
}
