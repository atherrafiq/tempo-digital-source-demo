namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestTableupdate2 : DbMigration
    {
        public override void Up()
        {
            return;
            DropColumn("dbo.Tests", "ReportID");
        }
        
        public override void Down()
        {
            return;
            AddColumn("dbo.Tests", "ReportID", c => c.Long(nullable: false));
        }
    }
}
