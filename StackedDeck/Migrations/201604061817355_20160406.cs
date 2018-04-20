namespace StackedDeck.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160406 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Credits");
            DropColumn("dbo.AspNetCreditRequests", "Amount");
            AddColumn("dbo.AspNetUsers", "Credits", c => c.Double(nullable: false, defaultValue: 1000));
            AddColumn("dbo.AspNetCreditRequests", "Amount", c => c.Double(nullable: false));

        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Credits");
            DropColumn("dbo.AspNetCreditRequests", "Amount");
            AddColumn("dbo.AspNetUsers", "Credits", c => c.Int(nullable: false, defaultValue: 1000));
            AddColumn("dbo.AspNetCreditRequests", "Amount", c => c.Int(nullable: false));
        }
    }
}
