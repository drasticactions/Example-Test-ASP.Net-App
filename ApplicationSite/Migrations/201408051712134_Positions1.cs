namespace ApplicationSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Positions1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        PositionStatus = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Positions");
        }
    }
}
