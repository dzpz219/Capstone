namespace StackedDeck.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160326 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.AspNetMessages",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Sender = c.String(nullable: false, maxLength: 50),
                   Recipient = c.String(nullable: false, maxLength: 50),
                   Content = c.String(),
                   MessageDate = c.DateTime(nullable: false),
               })
               .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.AspNetMessages");
        }
    }
}
