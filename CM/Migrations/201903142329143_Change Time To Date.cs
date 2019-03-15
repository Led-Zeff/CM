namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTimeToDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cas", "StartingDate", c => c.DateTime());
            AddColumn("dbo.Cas", "EndingDate", c => c.DateTime());
            DropColumn("dbo.Cas", "StartingTime");
            DropColumn("dbo.Cas", "EndingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cas", "EndingTime", c => c.DateTime());
            AddColumn("dbo.Cas", "StartingTime", c => c.DateTime());
            DropColumn("dbo.Cas", "EndingDate");
            DropColumn("dbo.Cas", "StartingDate");
        }
    }
}
