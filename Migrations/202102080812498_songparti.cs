namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class songparti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SongParticipents",
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
                        SongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipantID)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SongParticipents", "SongId", "dbo.Songs");
            DropIndex("dbo.SongParticipents", new[] { "SongId" });
            DropTable("dbo.SongParticipents");
        }
    }
}
