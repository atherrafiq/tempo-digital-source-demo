namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Napster_Streaming", "UserShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Napster_Streaming", "UPC", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Release_Label", c => c.String());
            AddColumn("dbo.Tidals", "UserShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Tidals", "UPC", c => c.String());
            AddColumn("dbo.Tidals", "Release_Label", c => c.String());
            DropColumn("dbo.Napster_Streaming", "share");
            DropColumn("dbo.Napster_Streaming", "Contract_Name");
            DropColumn("dbo.Napster_Streaming", "Type");
            DropColumn("dbo.Napster_Streaming", "Source");
            DropColumn("dbo.Napster_Streaming", "Sub_Source");
            DropColumn("dbo.Napster_Streaming", "Barcode");
            DropColumn("dbo.Napster_Streaming", "Cat_No");
            DropColumn("dbo.Napster_Streaming", "Distribution_Channel");
            DropColumn("dbo.Napster_Streaming", "Original_Currency");
            DropColumn("dbo.Tidals", "share");
            DropColumn("dbo.Tidals", "testtest_migration");
            DropColumn("dbo.Tidals", "Contract_Name");
            DropColumn("dbo.Tidals", "Type");
            DropColumn("dbo.Tidals", "Source");
            DropColumn("dbo.Tidals", "Sub_Source");
            DropColumn("dbo.Tidals", "Barcode");
            DropColumn("dbo.Tidals", "Cat_No");
            DropColumn("dbo.Tidals", "Distribution_Channel");
            DropColumn("dbo.Tidals", "Original_Currency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tidals", "Original_Currency", c => c.String());
            AddColumn("dbo.Tidals", "Distribution_Channel", c => c.String());
            AddColumn("dbo.Tidals", "Cat_No", c => c.String());
            AddColumn("dbo.Tidals", "Barcode", c => c.String());
            AddColumn("dbo.Tidals", "Sub_Source", c => c.String());
            AddColumn("dbo.Tidals", "Source", c => c.String());
            AddColumn("dbo.Tidals", "Type", c => c.String());
            AddColumn("dbo.Tidals", "Contract_Name", c => c.String());
            AddColumn("dbo.Tidals", "testtest_migration", c => c.String());
            AddColumn("dbo.Tidals", "share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Napster_Streaming", "Original_Currency", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Distribution_Channel", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Cat_No", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Barcode", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Sub_Source", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Source", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Type", c => c.String());
            AddColumn("dbo.Napster_Streaming", "Contract_Name", c => c.String());
            AddColumn("dbo.Napster_Streaming", "share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Tidals", "Release_Label");
            DropColumn("dbo.Tidals", "UPC");
            DropColumn("dbo.Tidals", "UserShare");
            DropColumn("dbo.Napster_Streaming", "Release_Label");
            DropColumn("dbo.Napster_Streaming", "UPC");
            DropColumn("dbo.Napster_Streaming", "UserShare");
        }
    }
}
