namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_AS_Source : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Napster_Streaming", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Napster_Streaming", "Source");
        }
    }
}
