namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "isYoutube", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "isDistribution", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "hasContract", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "isCustomUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "whoManagedUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "whoManagedUser");
            DropColumn("dbo.AspNetUsers", "isCustomUser");
            DropColumn("dbo.AspNetUsers", "hasContract");
            DropColumn("dbo.AspNetUsers", "isDistribution");
            DropColumn("dbo.AspNetUsers", "isYoutube");
            DropColumn("dbo.AspNetUsers", "isActive");
        }
    }
}
