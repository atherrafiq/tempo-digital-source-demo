namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wantYoutube : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "wantYoutube", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "wantYoutube");
        }
    }
}
