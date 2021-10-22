namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNulableFields : DbMigration
    {
        public override void Up()
        {
            return;
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Auction", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Reserved", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Partner_Sold_Youtube_Served", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Auction", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Reserved", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Partner_Sold_Youtube_Served", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Partner_Sold_Partner_Served", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            return;
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Partner_Sold_Partner_Served", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Partner_Sold_Youtube_Served", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Reserved", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "Partner_Revenue_Auction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Partner_Sold_Youtube_Served", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Reserved", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.YT_Reports_Music_Full", "YouTube_Revenue_Split_Auction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
