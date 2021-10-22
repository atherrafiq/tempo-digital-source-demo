namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserShareAdded : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.AspNetUsers", "share", c => c.String());
        }
        
        public override void Down()
        {
            return;
            DropColumn("dbo.AspNetUsers", "share");
        }
    }
}
