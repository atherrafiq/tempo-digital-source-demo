namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkfireLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "Linkfire_url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "Linkfire_url");
        }
    }
}
