namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCasFileProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasFile", "Name", c => c.String());
            AddColumn("dbo.CasFile", "DirectoryName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CasFile", "DirectoryName");
            DropColumn("dbo.CasFile", "Name");
        }
    }
}
