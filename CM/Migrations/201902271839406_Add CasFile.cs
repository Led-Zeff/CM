namespace CM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCasFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasFile",
                c => new
                    {
                        CasFileID = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        OriginalFile = c.Binary(),
                        ModifiedFile = c.Binary(),
                        CasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CasFileID)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .Index(t => t.CasId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CasFile", "CasId", "dbo.Cas");
            DropIndex("dbo.CasFile", new[] { "CasId" });
            DropTable("dbo.CasFile");
        }
    }
}
