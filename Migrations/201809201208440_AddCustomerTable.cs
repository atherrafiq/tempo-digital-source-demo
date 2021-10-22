namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "CustomerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "CustomerId");
        }
    }
}
