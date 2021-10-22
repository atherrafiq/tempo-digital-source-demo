namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NeedISRC : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.Songs", "needISRC", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            return;
            DropColumn("dbo.Songs", "needISRC");
        }
    }
}
