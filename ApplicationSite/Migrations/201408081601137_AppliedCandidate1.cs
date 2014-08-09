using System.Data.Entity.Migrations;

namespace ApplicationSite.Migrations
{
    public partial class AppliedCandidate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppliedCandidates", "AppliedTime", c => c.DateTime(false));
        }

        public override void Down()
        {
            DropColumn("dbo.AppliedCandidates", "AppliedTime");
        }
    }
}