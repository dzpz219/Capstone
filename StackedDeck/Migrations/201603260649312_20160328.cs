namespace StackedDeck.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160328 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetDiscussionThreads", "PostDate");
            DropColumn("dbo.AspNetDiscussionPosts", "PostDate");
            AddColumn("dbo.AspNetDiscussionThreads", "PostDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetDiscussionPosts", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
