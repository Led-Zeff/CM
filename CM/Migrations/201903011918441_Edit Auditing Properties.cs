namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditAuditingProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cas", "ModificationDate", c => c.DateTime());
            AlterColumn("dbo.CasFile", "ModificationDate", c => c.DateTime());
            AlterColumn("dbo.Project", "ModificationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "ModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CasFile", "ModificationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cas", "ModificationDate", c => c.DateTime(nullable: false));
        }
    }
}
