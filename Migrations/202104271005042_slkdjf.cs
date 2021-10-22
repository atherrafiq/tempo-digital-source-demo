namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slkdjf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonthlyPayments", "Is_Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonthlyPayments", "Is_Paid");
        }
    }
}
