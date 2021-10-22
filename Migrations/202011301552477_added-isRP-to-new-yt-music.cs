namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedisRPtonewytmusic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YT_Reports_Music_2020", "isRP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.YT_Reports_Music_2020", "isRP");
        }
    }
}
