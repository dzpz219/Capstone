namespace StackedDeck.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160330 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
             "dbo.AspNetReportedUsers",
             c => new
             {
                 Id = c.Int(nullable: false, identity: true),
                 PostId = c.Int(nullable: false),
                 ReportBy = c.String(nullable: false, maxLength: 50),
                 ReportDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
             })
             .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.AspNetReportedUsers");
        }
    }
}
