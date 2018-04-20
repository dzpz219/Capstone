using System;
using System.Collections.Generic;

namespace StackedDeck.Models
{
    public class DiscussionThread
    {
        public IEnumerable<DiscussionPost> Posts { get; set; }
        public IEnumerable<string> Replies { get; set; }

        public int Id { get; set; }
        public int PostCount { get; set; }
        public DateTime PostDate { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }

    public class DiscussionPost
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public DateTime PostDate { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
    }

}