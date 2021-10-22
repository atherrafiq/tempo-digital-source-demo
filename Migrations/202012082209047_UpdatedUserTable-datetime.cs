namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUserTabledatetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateTimeCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateTimeCreated");
        }
    }
}
