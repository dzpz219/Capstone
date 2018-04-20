using StackedDeck.Models;
using System.Collections.Generic;

namespace StackedDeck.ViewModels
{
    public class DiscussionViewModel
    {
        public IEnumerable<DiscussionThread> Threads{ get; set; }
        public IEnumerable<DiscussionPost> Posts { get; set; }
        public IEnumerable<string> ThreadCount { get; set; }
        public DiscussionThread Thread { get; set; }
        public DiscussionPost Post { get; set; }
    }
}