namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLAbel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apple_Music_New", "Release_Label", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apple_Music_New", "Release_Label");
        }
    }
}
