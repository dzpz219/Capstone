using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackedDeck.Models;

namespace StackedDeck.ViewModels
{
    public class MessageViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
    }
}