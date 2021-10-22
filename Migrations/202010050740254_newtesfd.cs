namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtesfd : DbMigration
    {
        public override void Up()
        {
            return;
            DropForeignKey("dbo.Tests", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropForeignKey("dbo.YT_Reports", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropForeignKey("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID", "dbo.YoutubeChannels");
            DropIndex("dbo.Tests", new[] { "YoutubeChannel_ID" });
            DropIndex("dbo.YT_Reports", new[] { "YoutubeChannel_ID" });
            DropIndex("dbo.YT_Reports_Entertainment_Full", new[] { "YoutubeChannel_ID" });
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        ParticipantID = c.Int(nullable: false, identity: true),
                        ParticipantName = c.String(),
                        ParticipantRole = c.String(),
                        RoleType = c.String(),
                        Instrument = c.String(),
                        IPICode = c.String(),
                        IPNCode = c.String(),
                        isPrimary = c.Boolean(),
                        needIPI = c.Boolean(),
                        needIPN = c.Boolean(),
                        needYoCopyToAllTracks = c.Boolean(),
                        MusicReleaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipantID)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
            CreateTable(
                "dbo.ISRC_Codes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ISRC = c.String(),
                        ISRC_status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MonthlyPayments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        MusicAmount = c.String(),
                        EntAmount = c.String(),
                        Date_Time = c.String(),
                        Music_Month = c.String(),
                        Music_Year = c.String(),
                        Ent_Month = c.String(),
                        Ent_Year = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Napster_Streaming",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        Contract_Name = c.String(),
                        Type = c.String(),
                        Sales_Date = c.String(),
                        Transaction_Date = c.String(),
                        Source = c.String(),
                        Sub_Source = c.String(),
                        Territory = c.String(),
                        Barcode = c.String(),
                        Cat_No = c.String(),
                        Release_Title = c.String(),
                        Release_Artist = c.String(),
                        ISRC = c.String(),
                        Track_Title = c.String(),
                        Track_Artist = c.String(),
                        Distribution_Channel = c.String(),
                        Configuration = c.String(),
                        Units = c.Int(nullable: false),
                        Original_Currency = c.String(),
                        Net_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pandoras",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        DSP = c.String(),
                        SalesDate = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        ReleaseTitle = c.String(),
                        ResourceTitle = c.String(),
                        Contributors = c.String(),
                        NumberOfConsumerSalesGross = c.Int(nullable: false),
                        CurrencyCode = c.String(),
                        EffectiveRoyaltyRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RoyaltyRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UseType = c.String(),
                        DistributionChannelType = c.String(),
                        ReleaseType = c.String(),
                        TerritoryCode = c.String(),
                        RightSharePercentage = c.String(),
                        PlanName = c.String(),
                        Member = c.String(),
                        LabelName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentRecords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User = c.String(),
                        MusicAmount = c.String(),
                        EntAmount = c.String(),
                        Date_Time = c.String(),
                        Music_Month = c.String(),
                        Music_Year = c.String(),
                        Ent_Month = c.String(),
                        Ent_Year = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tidals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        testtest_migration = c.String(),
                        Contract_Name = c.String(),
                        Type = c.String(),
                        Sales_Date = c.String(),
                        Transaction_Date = c.String(),
                        Source = c.String(),
                        Sub_Source = c.String(),
                        Territory = c.String(),
                        Barcode = c.String(),
                        Cat_No = c.String(),
                        Release_Title = c.String(),
                        Release_Artist = c.String(),
                        ISRC = c.String(),
                        Track_Title = c.String(),
                        Track_Artist = c.String(),
                        Distribution_Channel = c.String(),
                        Configuration = c.String(),
                        Units = c.Int(nullable: false),
                        Original_Currency = c.String(),
                        Net_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MyTestEF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TikToks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MusicReleaseId = c.Int(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        report_start_date = c.String(),
                        report_end_date = c.String(),
                        platform_name = c.String(),
                        content_provider = c.String(),
                        platform_song_id = c.String(),
                        song_title = c.String(),
                        artist = c.String(),
                        album = c.String(),
                        label_name = c.String(),
                        isrc = c.String(),
                        product_code = c.String(),
                        territory = c.String(),
                        video_creations = c.Int(nullable: false),
                        vv = c.Int(nullable: false),
                        statement_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        statement_currency = c.String(),
                        genre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UPC_Codes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UPC = c.String(),
                        UPC_status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        DateTime = c.String(),
                        FileName = c.String(),
                        FileLink = c.String(),
                        Other = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ActionPerformed = c.String(),
                        DateTime = c.String(),
                        UserName = c.String(),
                        UserEmail = c.String(),
                        ClientIP = c.String(),
                        Other = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.YoutubeConnects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        State = c.String(),
                        DOB = c.String(),
                        CurrentNetwork = c.String(),
                        AccountTitle = c.String(),
                        Descriptions = c.String(),
                        Status = c.String(),
                        Standing = c.String(),
                        Created = c.String(),
                        SubscribersCount = c.String(),
                        VideoCount = c.String(),
                        CommentCount = c.String(),
                        ViewCount = c.String(),
                        ImageURL = c.String(),
                        CommunityGuidelinesGS = c.String(),
                        ContentIdClaimsGS = c.String(),
                        CopyrightStrikesGS = c.String(),
                        PublishedTime = c.String(),
                        Country = c.String(),
                        AccountStatus = c.String(),
                        ChannelID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.YT_Reports_Entertainment_Full_New",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        Adjustment_Type = c.String(),
                        Asset_ID = c.String(),
                        Asset_Title = c.String(),
                        Asset_Labels = c.String(),
                        Asset_Channel_ID = c.String(),
                        Asset_Type = c.String(),
                        Custom_ID = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        GRid = c.String(),
                        Artist = c.String(),
                        Album = c.String(),
                        Label = c.String(),
                        Administer_Publish_Right = c.String(),
                        Owned_Views = c.Int(nullable: false),
                        YouTube_Revenue_Split_Auction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split_Reserved = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split_Partner_Sold_Youtube_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YouTube_Revenue_Split = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Auction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Reserved = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Partner_Sold_Youtube_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue_Partner_Sold_Partner_Served = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Partner_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        userShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannelsENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.YoutubeChannelsENTs", t => t.YoutubeChannelsENT_ID)
                .Index(t => t.YoutubeChannelsENT_ID);
            
            CreateTable(
                "dbo.YT_Reports_Ent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        Adjustment_Type = c.String(),
                        Asset_ID = c.String(),
                        Asset_Title = c.String(),
                        Asset_Labels = c.String(),
                        Asset_Channel_ID = c.String(),
                        Asset_Type = c.String(),
                        Custom_ID = c.String(),
                        ISRC = c.String(),
                        UPC = c.String(),
                        GRid = c.String(),
                        Artist = c.String(),
                        Album = c.String(),
                        Label = c.String(),
                        Owned_Views = c.Int(nullable: false),
                        Partner_Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        userShare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannelsENT_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.YoutubeChannelsENTs", t => t.YoutubeChannelsENT_ID)
                .Index(t => t.YoutubeChannelsENT_ID);
            
            AddColumn("dbo.MusicReleases", "isVA", c => c.Boolean(nullable: false));
            AddColumn("dbo.MusicReleases", "Status", c => c.String());
            AddColumn("dbo.MusicReleases", "DeletionDate", c => c.String());
            AddColumn("dbo.MusicReleases", "PaymentStaus", c => c.String());
            AddColumn("dbo.MusicReleases", "ReleaseFormat", c => c.String());
            AddColumn("dbo.MusicReleases", "ReleaseVersion", c => c.String());
            AddColumn("dbo.MusicReleases", "ReleaseCategory", c => c.String());
            AddColumn("dbo.MusicReleases", "ReleaseDate", c => c.String());
            AddColumn("dbo.MusicReleases", "OriginalReleaseDate", c => c.String());
            AddColumn("dbo.MusicReleases", "MetadataLanguage", c => c.String());
            AddColumn("dbo.MusicReleases", "ReleaseAdvisory", c => c.String());
            AddColumn("dbo.MusicReleases", "ImageFormat", c => c.String());
            AddColumn("dbo.MusicReleases", "TerritoryAvailability", c => c.String());
            AddColumn("dbo.MusicReleases", "CInfo", c => c.String());
            AddColumn("dbo.MusicReleases", "CYear", c => c.String());
            AddColumn("dbo.MusicReleases", "PInfo", c => c.String());
            AddColumn("dbo.MusicReleases", "PYear", c => c.String());
            AddColumn("dbo.Songs", "isVAR", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "ArtistName", c => c.String());
            AddColumn("dbo.Songs", "ComposerName", c => c.String());
            AddColumn("dbo.Songs", "WriterName", c => c.String());
            AddColumn("dbo.Songs", "ArrangerName", c => c.String());
            AddColumn("dbo.Songs", "CInfo", c => c.String());
            AddColumn("dbo.Songs", "CYear", c => c.String());
            AddColumn("dbo.Songs", "PInfo", c => c.String());
            AddColumn("dbo.Songs", "PYear", c => c.String());
            AddColumn("dbo.Songs", "ISWC", c => c.String());
            AddColumn("dbo.Songs", "Advisory", c => c.String());
            AddColumn("dbo.Songs", "PermitDownload", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "PermitStreaming", c => c.Boolean(nullable: false));
            AddColumn("dbo.Songs", "DownloadDate", c => c.DateTime());
            AddColumn("dbo.Songs", "StreamingDate", c => c.DateTime());
            AddColumn("dbo.Stores", "iTunesReleasePriceTier", c => c.String());
            AddColumn("dbo.Stores", "iTunesTrackPriceTier", c => c.String());
            AddColumn("dbo.Stores", "updateToNewStores", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stores", "isYoutubeSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stores", "isFacebookSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Bank", c => c.String());
            AddColumn("dbo.AspNetUsers", "Paypal", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankName", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankIBAN", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankCountry", c => c.String());
            AddColumn("dbo.AspNetUsers", "BankEmail", c => c.String());
            AddColumn("dbo.AspNetUsers", "CompanyName", c => c.String());
            AddColumn("dbo.AspNetUsers", "CompanyAddress", c => c.String());
            AddColumn("dbo.AspNetUsers", "Contact", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserType", c => c.String());
            AddColumn("dbo.Deezers", "Title", c => c.String());
            AddColumn("dbo.Deezers", "Service", c => c.String());
            AddColumn("dbo.Deezers", "Share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.YoutubeChannelsENTs", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.YoutubeChannelsENTs", "ApprovelDate", c => c.String());
            AddColumn("dbo.Google_Play", "Share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SoundClouds", "Share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SpotifyReportFulls", "share", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.YoutubeChannels", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.YoutubeChannels", "ApprovelDate", c => c.String());
            AddColumn("dbo.YT_Reports_Entertainment_Full", "userShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.YT_Reports_Music_Full", "RedPartner", c => c.String());
            AddColumn("dbo.YT_Reports_Music_Full", "userShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Google_Play", "MusicReleaseId", c => c.Int());
            DropColumn("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID");
            DropTable("dbo.Tests");
            DropTable("dbo.YT_Reports");
        }
        
        public override void Down()
        {
            return;
            CreateTable(
                "dbo.YT_Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        ChannelName = c.String(),
                        ChannelID = c.String(),
                        AssetTitle = c.String(),
                        AssetType = c.String(),
                        Views = c.Int(nullable: false),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.String(),
                        Year = c.String(),
                        ChannelName = c.String(),
                        ChannelID = c.String(),
                        AssetTitle = c.String(),
                        AssetType = c.String(),
                        Views = c.Int(nullable: false),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                        YoutubeChannel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID", c => c.Int());
            DropForeignKey("dbo.YT_Reports_Ent", "YoutubeChannelsENT_ID", "dbo.YoutubeChannelsENTs");
            DropForeignKey("dbo.YT_Reports_Entertainment_Full_New", "YoutubeChannelsENT_ID", "dbo.YoutubeChannelsENTs");
            DropForeignKey("dbo.Participants", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.YT_Reports_Ent", new[] { "YoutubeChannelsENT_ID" });
            DropIndex("dbo.YT_Reports_Entertainment_Full_New", new[] { "YoutubeChannelsENT_ID" });
            DropIndex("dbo.Participants", new[] { "MusicReleaseId" });
            AlterColumn("dbo.Google_Play", "MusicReleaseId", c => c.Int(nullable: false));
            DropColumn("dbo.YT_Reports_Music_Full", "userShare");
            DropColumn("dbo.YT_Reports_Music_Full", "RedPartner");
            DropColumn("dbo.YT_Reports_Entertainment_Full", "userShare");
            DropColumn("dbo.YoutubeChannels", "ApprovelDate");
            DropColumn("dbo.YoutubeChannels", "Approved");
            DropColumn("dbo.SpotifyReportFulls", "share");
            DropColumn("dbo.SoundClouds", "Share");
            DropColumn("dbo.Google_Play", "Share");
            DropColumn("dbo.YoutubeChannelsENTs", "ApprovelDate");
            DropColumn("dbo.YoutubeChannelsENTs", "Approved");
            DropColumn("dbo.Deezers", "Share");
            DropColumn("dbo.Deezers", "Service");
            DropColumn("dbo.Deezers", "Title");
            DropColumn("dbo.AspNetUsers", "UserType");
            DropColumn("dbo.AspNetUsers", "Contact");
            DropColumn("dbo.AspNetUsers", "CompanyAddress");
            DropColumn("dbo.AspNetUsers", "CompanyName");
            DropColumn("dbo.AspNetUsers", "BankEmail");
            DropColumn("dbo.AspNetUsers", "BankCountry");
            DropColumn("dbo.AspNetUsers", "BankIBAN");
            DropColumn("dbo.AspNetUsers", "BankAddress");
            DropColumn("dbo.AspNetUsers", "BankName");
            DropColumn("dbo.AspNetUsers", "Paypal");
            DropColumn("dbo.AspNetUsers", "Bank");
            DropColumn("dbo.Stores", "isFacebookSelected");
            DropColumn("dbo.Stores", "isYoutubeSelected");
            DropColumn("dbo.Stores", "updateToNewStores");
            DropColumn("dbo.Stores", "iTunesTrackPriceTier");
            DropColumn("dbo.Stores", "iTunesReleasePriceTier");
            DropColumn("dbo.Songs", "StreamingDate");
            DropColumn("dbo.Songs", "DownloadDate");
            DropColumn("dbo.Songs", "PermitStreaming");
            DropColumn("dbo.Songs", "PermitDownload");
            DropColumn("dbo.Songs", "Advisory");
            DropColumn("dbo.Songs", "ISWC");
            DropColumn("dbo.Songs", "PYear");
            DropColumn("dbo.Songs", "PInfo");
            DropColumn("dbo.Songs", "CYear");
            DropColumn("dbo.Songs", "CInfo");
            DropColumn("dbo.Songs", "ArrangerName");
            DropColumn("dbo.Songs", "WriterName");
            DropColumn("dbo.Songs", "ComposerName");
            DropColumn("dbo.Songs", "ArtistName");
            DropColumn("dbo.Songs", "isVAR");
            DropColumn("dbo.MusicReleases", "PYear");
            DropColumn("dbo.MusicReleases", "PInfo");
            DropColumn("dbo.MusicReleases", "CYear");
            DropColumn("dbo.MusicReleases", "CInfo");
            DropColumn("dbo.MusicReleases", "TerritoryAvailability");
            DropColumn("dbo.MusicReleases", "ImageFormat");
            DropColumn("dbo.MusicReleases", "ReleaseAdvisory");
            DropColumn("dbo.MusicReleases", "MetadataLanguage");
            DropColumn("dbo.MusicReleases", "OriginalReleaseDate");
            DropColumn("dbo.MusicReleases", "ReleaseDate");
            DropColumn("dbo.MusicReleases", "ReleaseCategory");
            DropColumn("dbo.MusicReleases", "ReleaseVersion");
            DropColumn("dbo.MusicReleases", "ReleaseFormat");
            DropColumn("dbo.MusicReleases", "PaymentStaus");
            DropColumn("dbo.MusicReleases", "DeletionDate");
            DropColumn("dbo.MusicReleases", "Status");
            DropColumn("dbo.MusicReleases", "isVA");
            DropTable("dbo.YT_Reports_Ent");
            DropTable("dbo.YT_Reports_Entertainment_Full_New");
            DropTable("dbo.YoutubeConnects");
            DropTable("dbo.UserLogs");
            DropTable("dbo.UserFiles");
            DropTable("dbo.UPC_Codes");
            DropTable("dbo.TikToks");
            DropTable("dbo.Tidals");
            DropTable("dbo.PaymentRecords");
            DropTable("dbo.Pandoras");
            DropTable("dbo.Napster_Streaming");
            DropTable("dbo.MonthlyPayments");
            DropTable("dbo.ISRC_Codes");
            DropTable("dbo.Participants");
            CreateIndex("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID");
            CreateIndex("dbo.YT_Reports", "YoutubeChannel_ID");
            CreateIndex("dbo.Tests", "YoutubeChannel_ID");
            AddForeignKey("dbo.YT_Reports_Entertainment_Full", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
            AddForeignKey("dbo.YT_Reports", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
            AddForeignKey("dbo.Tests", "YoutubeChannel_ID", "dbo.YoutubeChannels", "ID");
        }
    }
}
