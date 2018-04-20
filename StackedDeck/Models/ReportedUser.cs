using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    public class ReportedUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public string ReportBy { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ReportDate { get; set; }
    }
}