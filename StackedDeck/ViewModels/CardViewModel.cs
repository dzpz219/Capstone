using StackedDeck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.ViewModels
{
    public class CardViewModel
    {
        public Stack<Card> DealerHand { get; set; }
        public Stack<Card> PlayerHand { get; set; }
        public Card Card { get; set; }
    }
}