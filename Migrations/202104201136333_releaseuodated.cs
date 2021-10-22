namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class releaseuodated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "artistToAllSongs", c => c.Boolean(nullable: false));
            AddColumn("dbo.MusicReleases", "copyRightToAllSongs", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "copyRightToAllSongs");
            DropColumn("dbo.MusicReleases", "artistToAllSongs");
        }
    }
}
