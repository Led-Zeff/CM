namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuditingProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cas", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cas", "ModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CasFile", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CasFile", "ModificationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Project", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Project", "ModificationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "ModificationDate");
            DropColumn("dbo.Project", "RegistrationDate");
            DropColumn("dbo.CasFile", "ModificationDate");
            DropColumn("dbo.CasFile", "RegistrationDate");
            DropColumn("dbo.Cas", "ModificationDate");
            DropColumn("dbo.Cas", "RegistrationDate");
        }
    }
}
