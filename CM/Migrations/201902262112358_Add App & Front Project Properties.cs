namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppFrontProjectProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "LocalAppPublishPath", c => c.String());
            AddColumn("dbo.Project", "LocalFrontPublishPath", c => c.String());
            AddColumn("dbo.Project", "DevelopmentAppPublishPath", c => c.String());
            AddColumn("dbo.Project", "DevelopmentFrontPublishPath", c => c.String());
            DropColumn("dbo.Project", "LocalPublishPath");
            DropColumn("dbo.Project", "DevelopmentPublishPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "DevelopmentPublishPath", c => c.String());
            AddColumn("dbo.Project", "LocalPublishPath", c => c.String());
            DropColumn("dbo.Project", "DevelopmentFrontPublishPath");
            DropColumn("dbo.Project", "DevelopmentAppPublishPath");
            DropColumn("dbo.Project", "LocalFrontPublishPath");
            DropColumn("dbo.Project", "LocalAppPublishPath");
        }
    }
}
