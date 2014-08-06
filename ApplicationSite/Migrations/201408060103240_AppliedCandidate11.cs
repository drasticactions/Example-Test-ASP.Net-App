namespace ApplicationSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppliedCandidate11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppliedCandidate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppliedCandidateState = c.Int(nullable: false),
                        Position_Id = c.Int(),
                        Resume_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Position_Id)
                .Index(t => t.Resume_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppliedCandidate", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AppliedCandidate", new[] { "User_Id" });
            DropTable("dbo.AppliedCandidate");
        }
    }
}
