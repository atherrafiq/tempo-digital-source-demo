namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeChannelTableFixed2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.YoutubeChannels", new[] { "User_Id" });
            DropColumn("dbo.YoutubeChannels", "UserId");
            RenameColumn(table: "dbo.YoutubeChannels", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.YoutubeChannels", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.YoutubeChannels", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.YoutubeChannels", new[] { "UserId" });
            AlterColumn("dbo.YoutubeChannels", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.YoutubeChannels", name: "UserId", newName: "User_Id");
            AddColumn("dbo.YoutubeChannels", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.YoutubeChannels", "User_Id");
        }
    }
}
