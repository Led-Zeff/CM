namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "DevPublishPathUser", c => c.String());
            AddColumn("dbo.Project", "DevPublishPathPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "DevPublishPathPassword");
            DropColumn("dbo.Project", "DevPublishPathUser");
        }
    }
}
