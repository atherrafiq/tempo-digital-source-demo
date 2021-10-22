namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Codes : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.UPC_ISRC",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UPC = c.String(),
                        UPC_status = c.String(),
                        ISRC = c.String(),
                        ISRC_status = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            return;
            DropTable("dbo.UPC_ISRC");
        }
    }
}
