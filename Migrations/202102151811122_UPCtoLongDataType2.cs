namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPCtoLongDataType2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UPC_Codes", "UPC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UPC_Codes", "UPC", c => c.Long(nullable: false));
        }
    }
}
