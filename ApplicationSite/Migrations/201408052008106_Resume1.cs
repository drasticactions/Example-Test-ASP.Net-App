namespace ApplicationSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Resume1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resume",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    FileName = c.String(nullable: false),
                    Path = c.String(nullable: false),
                    UserId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

        }
        
        public override void Down()
        {
            DropTable("dbo.Resume");
        }
    }
}
