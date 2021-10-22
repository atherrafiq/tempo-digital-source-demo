namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeChannelTableFixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YoutubeChannels", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.YoutubeChannels", "ID_Channel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.YoutubeChannels", "ID_Channel", c => c.Int(nullable: false));
            DropColumn("dbo.YoutubeChannels", "UserId");
        }
    }
}
