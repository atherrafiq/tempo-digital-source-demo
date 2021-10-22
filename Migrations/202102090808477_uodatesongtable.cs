namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uodatesongtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "CopyRightHolder", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "CopyRightHolder");
        }
    }
}
