namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelativePathToCasFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasFile", "RelativePath", c => c.String());
            DropColumn("dbo.CasFile", "Path");
            DropColumn("dbo.CasFile", "DirectoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CasFile", "DirectoryName", c => c.String());
            AddColumn("dbo.CasFile", "Path", c => c.String());
            DropColumn("dbo.CasFile", "RelativePath");
        }
    }
}
