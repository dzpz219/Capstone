namespace StackedDeck.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160329 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
              "dbo.AspNetCreditRequests",
              c => new
              {
                  Id = c.Int(nullable: false, identity: true),
                  Username = c.String(nullable: false, maxLength: 50),
                  Amount = c.Int(nullable: false, defaultValue: 0),
                  Approved = c.Boolean(nullable: false, defaultValue: false),
                  RequestDate = c.DateTime(nullable: false),
                  ApprovalDate = c.DateTime(nullable: true),
              })
              .PrimaryKey(t => t.Id);

            AddColumn("dbo.AspNetUsers", "Credits", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropTable("dbo.AspNetCredits");
            DropColumn("dbo.AspNetUsers", "Credits");
        }
    }
}
