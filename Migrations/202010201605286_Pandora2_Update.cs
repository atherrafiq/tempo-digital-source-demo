namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pandora2_Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pandoras", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.Pandoras", new[] { "YoutubeChannel_ID" });
            DropPrimaryKey("dbo.Pandoras");
            AddColumn("dbo.Pandoras", "testtest_migration", c => c.String());
            AddColumn("dbo.Pandoras", "Contract_Name", c => c.String());
            AddColumn("dbo.Pandoras", "Type", c => c.String());
            AddColumn("dbo.Pandoras", "Sales_Date", c => c.String());
            AddColumn("dbo.Pandoras", "Transaction_Date", c => c.String());
            AddColumn("dbo.Pandoras", "Source", c => c.String());
            AddColumn("dbo.Pandoras", "Sub_Source", c => c.String());
            AddColumn("dbo.Pandoras", "Territory", c => c.String());
            AddColumn("dbo.Pandoras", "Barcode", c => c.String());
            AddColumn("dbo.Pandoras", "Cat_No", c => c.String());
            AddColumn("dbo.Pandoras", "Release_Title", c => c.String());
            AddColumn("dbo.Pandoras", "Release_Artist", c => c.String());
            AddColumn("dbo.Pandoras", "Track_Title", c => c.String());
            AddColumn("dbo.Pandoras", "Track_Artist", c => c.String());
            AddColumn("dbo.Pandoras", "Distribution_Channel", c => c.String());
            AddColumn("dbo.Pandoras", "Configuration", c => c.String());
            AddColumn("dbo.Pandoras", "Units", c => c.Int(nullable: false));
            AddColumn("dbo.Pandoras", "Original_Currency", c => c.String());
            AddColumn("dbo.Pandoras", "Net_Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Pandoras", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Pandoras", "Id");
            DropColumn("dbo.Pandoras", "DSP");
            DropColumn("dbo.Pandoras", "SalesDate");
            DropColumn("dbo.Pandoras", "UPC");
            DropColumn("dbo.Pandoras", "ReleaseTitle");
            DropColumn("dbo.Pandoras", "ResourceTitle");
            DropColumn("dbo.Pandoras", "Contributors");
            DropColumn("dbo.Pandoras", "NumberOfConsumerSalesGross");
            DropColumn("dbo.Pandoras", "CurrencyCode");
            DropColumn("dbo.Pandoras", "EffectiveRoyaltyRate");
            DropColumn("dbo.Pandoras", "RoyaltyRate");
            DropColumn("dbo.Pandoras", "UseType");
            DropColumn("dbo.Pandoras", "DistributionChannelType");
            DropColumn("dbo.Pandoras", "ReleaseType");
            DropColumn("dbo.Pandoras", "TerritoryCode");
            DropColumn("dbo.Pandoras", "RightSharePercentage");
            DropColumn("dbo.Pandoras", "PlanName");
            DropColumn("dbo.Pandoras", "Member");
            DropColumn("dbo.Pandoras", "LabelName");
            DropColumn("dbo.Pandoras", "YoutubeChannel_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pandoras", "YoutubeChannel_ID", c => c.Int());
            AddColumn("dbo.Pandoras", "LabelName", c => c.String());
            AddColumn("dbo.Pandoras", "Member", c => c.String());
            AddColumn("dbo.Pandoras", "PlanName", c => c.String());
            AddColumn("dbo.Pandoras", "RightSharePercentage", c => c.String());
            AddColumn("dbo.Pandoras", "TerritoryCode", c => c.String());
            AddColumn("dbo.Pandoras", "ReleaseType", c => c.String());
            AddColumn("dbo.Pandoras", "DistributionChannelType", c => c.String());
            AddColumn("dbo.Pandoras", "UseType", c => c.String());
            AddColumn("dbo.Pandoras", "RoyaltyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pandoras", "EffectiveRoyaltyRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pandoras", "CurrencyCode", c => c.String());
            AddColumn("dbo.Pandoras", "NumberOfConsumerSalesGross", c => c.Int(nullable: false));
            AddColumn("dbo.Pandoras", "Contributors", c => c.String());
            AddColumn("dbo.Pandoras", "ResourceTitle", c => c.String());
            AddColumn("dbo.Pandoras", "ReleaseTitle", c => c.String());
            AddColumn("dbo.Pandoras", "UPC", c => c.String());
            AddColumn("dbo.Pandoras", "SalesDate", c => c.String());
            AddColumn("dbo.Pandoras", "DSP", c => c.String());
            DropPrimaryKey("dbo.Pandoras");
            AlterColumn("dbo.Pandoras", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Pandoras", "Net_Amount");
            DropColumn("dbo.Pandoras", "Original_Currency");
            DropColumn("dbo.Pandoras", "Units");
            DropColumn("dbo.Pandoras", "Configuration");
            DropColumn("dbo.Pandoras", "Distribution_Channel");
            DropColumn("dbo.Pandoras", "Track_Artist");
            DropColumn("dbo.Pandoras", "Track_Title");
            DropColumn("dbo.Pandoras", "Release_Artist");
            DropColumn("dbo.Pandoras", "Release_Title");
            DropColumn("dbo.Pandoras", "Cat_No");
            DropColumn("dbo.Pandoras", "Barcode");
            DropColumn("dbo.Pandoras", "Territory");
            DropColumn("dbo.Pandoras", "Sub_Source");
            DropColumn("dbo.Pandoras", "Source");
            DropColumn("dbo.Pandoras", "Transaction_Date");
            DropColumn("dbo.Pandoras", "Sales_Date");
            DropColumn("dbo.Pandoras", "Type");
            DropColumn("dbo.Pandoras", "Contract_Name");
            DropColumn("dbo.Pandoras", "testtest_migration");
            AddPrimaryKey("dbo.Pandoras", "Id");
            CreateIndex("dbo.Pandoras", "YoutubeChannel_ID");
            AddForeignKey("dbo.Pandoras", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
        }
    }
}
