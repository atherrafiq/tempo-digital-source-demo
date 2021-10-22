namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPCtoLongDataType4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UPC_Codes", "UPC", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UPC_Codes", "UPC");
        }
    }
}
