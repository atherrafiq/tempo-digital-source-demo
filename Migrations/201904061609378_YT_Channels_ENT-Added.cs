namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YT_Channels_ENTAdded : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.YoutubeChannelsENTs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChannelName = c.String(),
                        ChannelID = c.String(),
                        Description = c.String(),
                        YT_Label_Name = c.String(),
                        YT_Label_Description = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            return;
            DropForeignKey("dbo.YoutubeChannelsENTs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.YoutubeChannelsENTs", new[] { "UserId" });
            DropTable("dbo.YoutubeChannelsENTs");
        }
    }
}
