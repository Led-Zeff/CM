namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCasFileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasFile", "FileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CasFile", "FileType");
        }
    }
}
