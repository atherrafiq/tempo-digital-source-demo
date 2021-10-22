namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class double_ : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpotifyReports_2020", "USD_Payable", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpotifyReports_2020", "USD_Payable", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
