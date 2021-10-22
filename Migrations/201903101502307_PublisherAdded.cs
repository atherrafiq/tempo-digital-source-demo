namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublisherAdded : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.MusicReleases", "Publisher", c => c.String());
        }
        
        public override void Down()
        {
            return;
            DropColumn("dbo.MusicReleases", "Publisher");
        }
    }
}
