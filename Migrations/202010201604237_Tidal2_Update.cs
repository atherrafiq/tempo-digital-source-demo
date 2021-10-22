namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tidal2_Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tidals", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.Tidals", new[] { "YoutubeChannel_ID" });
            DropPrimaryKey("dbo.Tidals");
            AlterColumn("dbo.Tidals", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tidals", "Id");
            DropColumn("dbo.Tidals", "MyTestEF");
            DropColumn("dbo.Tidals", "YoutubeChannel_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tidals", "YoutubeChannel_ID", c => c.Int());
            AddColumn("dbo.Tidals", "MyTestEF", c => c.String());
            DropPrimaryKey("dbo.Tidals");
            AlterColumn("dbo.Tidals", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tidals", "Id");
            CreateIndex("dbo.Tidals", "YoutubeChannel_ID");
            AddForeignKey("dbo.Tidals", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
        }
    }
}
