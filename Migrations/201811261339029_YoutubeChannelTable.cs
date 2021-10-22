namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeChannelTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YoutubeChannels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ID_Channel = c.Int(nullable: false),
                        ChannelName = c.String(),
                        ChannnelUrl = c.String(),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YoutubeChannels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.YoutubeChannels", new[] { "User_Id" });
            DropTable("dbo.YoutubeChannels");
        }
    }
}
