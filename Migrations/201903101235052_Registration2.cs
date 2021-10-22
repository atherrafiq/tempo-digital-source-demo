namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Registration2 : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LabelName", c => c.String());
            AddColumn("dbo.AspNetUsers", "AtristName", c => c.String());
        }
        
        public override void Down()
        {
            return;
            DropColumn("dbo.AspNetUsers", "AtristName");
            DropColumn("dbo.AspNetUsers", "LabelName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
