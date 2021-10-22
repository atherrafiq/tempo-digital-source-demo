namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatereleasecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "Update_Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MusicReleases", "Update_Status");
        }
    }
}
