namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDeletedtoCasFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasFile", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CasFile", "IsDeleted");
        }
    }
}
