using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public int Replies { get; set; }
        public string Content { get; set; }
        public DateTime MessageDate { get; set; }
    }
}