namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MusicReleases", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MusicReleases", "UserId");
            AddForeignKey("dbo.MusicReleases", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MusicReleases", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.MusicReleases", new[] { "UserId" });
            DropColumn("dbo.MusicReleases", "UserId");
        }
    }
}
