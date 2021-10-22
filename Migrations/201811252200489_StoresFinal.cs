namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoresFinal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                        StoreName = c.String(),
                        isSelected = c.Boolean(nullable: false),
                        MusicReleaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoreId)
                .ForeignKey("dbo.MusicReleases", t => t.MusicReleaseId, cascadeDelete: true)
                .Index(t => t.MusicReleaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Stores", new[] { "MusicReleaseId" });
            DropTable("dbo.Stores");
        }
    }
}
