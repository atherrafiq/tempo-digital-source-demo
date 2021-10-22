namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedisRPtonewytent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.YT_Reports_Ent_2020", "isRP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.YT_Reports_Ent_2020", "isRP");
        }
    }
}
