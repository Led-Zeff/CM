namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cas",
                c => new
                    {
                        CasID = c.Int(nullable: false, identity: true),
                        Number = c.Int(),
                        Description = c.String(),
                        StartingTime = c.DateTime(),
                        EndingTime = c.DateTime(),
                        ProjectID = c.Int(),
                    })
                .PrimaryKey(t => t.CasID)
                .ForeignKey("dbo.Project", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DevelopmentUrl = c.String(),
                        DBConnectionString = c.String(),
                        LocalPublishPath = c.String(),
                        DevelopmentPublishPath = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cas", "ProjectID", "dbo.Project");
            DropIndex("dbo.Cas", new[] { "ProjectID" });
            DropTable("dbo.Project");
            DropTable("dbo.Cas");
        }
    }
}
