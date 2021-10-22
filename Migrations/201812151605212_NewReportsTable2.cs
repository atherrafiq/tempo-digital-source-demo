namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewReportsTable2 : DbMigration
    {
        public override void Up()
        {
            return;
            DropColumn("dbo.YT_Reports", "ReportID");
        }
        
        public override void Down()
        {
            return;
            AddColumn("dbo.YT_Reports", "ReportID", c => c.Long(nullable: false));
        }
    }
}
