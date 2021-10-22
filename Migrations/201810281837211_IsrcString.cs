namespace tpdigit_asp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsrcString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Songs", "isrc", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Songs", "isrc", c => c.Int(nullable: false));
        }
    }
}
