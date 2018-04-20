namespace StackedDeck.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _20160315 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetDiscussionThreads",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Author = c.String(nullable: false, maxLength: 256),
                    Topic = c.String(nullable: false, maxLength: 256),
                    Content = c.String(),
                    PostDate = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetDiscussionPosts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ThreadId = c.Int(nullable: false),
                    Username = c.String(nullable: false, maxLength: 256),
                    Content = c.String(),
                    PostDate = c.String(nullable: false, maxLength: 50),
                    Avatar = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.AspNetDiscussionThreads");
            DropTable("dbo.AspNetDiscussionPosts");
        }
    }
}
