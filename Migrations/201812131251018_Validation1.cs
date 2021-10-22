namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Validation1 : DbMigration
    {
        public override void Up()
        {
            return;
            AlterColumn("dbo.MusicReleases", "ReleaseName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            return;
            AlterColumn("dbo.MusicReleases", "ReleaseName", c => c.String());
        }
    }
}
