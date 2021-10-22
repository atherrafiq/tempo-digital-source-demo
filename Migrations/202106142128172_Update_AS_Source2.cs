namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_AS_Source2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apple_Music_New", "Source", c => c.String());
            AddColumn("dbo.Tidals", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tidals", "Source");
            DropColumn("dbo.Apple_Music_New", "Source");
        }
    }
}
