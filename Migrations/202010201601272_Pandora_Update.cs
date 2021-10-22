namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pandora_Update : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Pandoras");
            AddColumn("dbo.Pandoras", "YoutubeChannel_ID", c => c.Int());
            AlterColumn("dbo.Pandoras", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Pandoras", "Id");
            CreateIndex("dbo.Pandoras", "YoutubeChannel_ID");
            AddForeignKey("dbo.Pandoras", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pandoras", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.Pandoras", new[] { "YoutubeChannel_ID" });
            DropPrimaryKey("dbo.Pandoras");
            AlterColumn("dbo.Pandoras", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.Pandoras", "YoutubeChannel_ID");
            AddPrimaryKey("dbo.Pandoras", "Id");
        }
    }
}
