namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class needUPC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "needUPC", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "needUPC");
        }
    }
}
