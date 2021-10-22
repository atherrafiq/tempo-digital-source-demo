namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class releaseuodateditunes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "Itune_Track_Price", c => c.String());
            AddColumn("dbo.MusicReleases", "Itune_Release_Price", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "Itune_Release_Price");
            DropColumn("dbo.MusicReleases", "Itune_Track_Price");
        }
    }
}
