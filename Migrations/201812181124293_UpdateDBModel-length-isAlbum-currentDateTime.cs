namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDBModellengthisAlbumcurrentDateTime : DbMigration
    {
        public override void Up()
        {
            return;
            AddColumn("dbo.MusicReleases", "isAlbum", c => c.Boolean(nullable: false));
            AddColumn("dbo.MusicReleases", "currentDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Songs", "Length", c => c.String());
            AlterColumn("dbo.MusicReleases", "ReleaseName", c => c.String());
        }
        
        public override void Down()
        {
            return;
            AlterColumn("dbo.MusicReleases", "ReleaseName", c => c.String(nullable: false));
            DropColumn("dbo.Songs", "Length");
            DropColumn("dbo.MusicReleases", "currentDateTime");
            DropColumn("dbo.MusicReleases", "isAlbum");
        }
    }
}
