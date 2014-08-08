namespace ApplicationSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppliedCandidate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppliedCandidates", "AppliedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppliedCandidates", "AppliedTime");
        }
    }
}
