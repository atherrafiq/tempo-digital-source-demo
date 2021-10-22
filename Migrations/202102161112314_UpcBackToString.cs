namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpcBackToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UPC_Codes", "UPC", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UPC_Codes", "UPC", c => c.Long(nullable: false));
        }
    }
}
