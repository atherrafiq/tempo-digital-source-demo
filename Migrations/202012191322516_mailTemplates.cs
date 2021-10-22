namespace TempoDigitalApex3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mailTemplates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.mail_templates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        template = c.String(),
                        other = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.mail_templates");
        }
    }
}
