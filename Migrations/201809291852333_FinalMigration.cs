namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Songs", "MusicRelease_MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Songs", new[] { "MusicRelease_MusicReleaseId" });
            RenameColumn(table: "dbo.Songs", name: "MusicRelease_MusicReleaseId", newName: "MusicReleaseId");
            AlterColumn("dbo.Songs", "MusicReleaseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Songs", "MusicReleaseId");
            AddForeignKey("dbo.Songs", "MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId", cascadeDelete: true);
            DropColumn("dbo.Songs", "MusicReleiseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "MusicReleiseId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Songs", "MusicReleaseId", "dbo.MusicReleases");
            DropIndex("dbo.Songs", new[] { "MusicReleaseId" });
            AlterColumn("dbo.Songs", "MusicReleaseId", c => c.Int());
            RenameColumn(table: "dbo.Songs", name: "MusicReleaseId", newName: "MusicRelease_MusicReleaseId");
            CreateIndex("dbo.Songs", "MusicRelease_MusicReleaseId");
            AddForeignKey("dbo.Songs", "MusicRelease_MusicReleaseId", "dbo.MusicReleases", "MusicReleaseId");
        }
    }
}
