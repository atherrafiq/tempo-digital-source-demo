namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoverImageAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "CoverImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "CoverImage");
        }
    }
}
