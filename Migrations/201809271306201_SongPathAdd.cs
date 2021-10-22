namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongPathAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "SongPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "SongPath");
        }
    }
}
