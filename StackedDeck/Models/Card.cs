using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    public class Card
    {
        public int Value { get; set; }
        public string Suit { get; set; }
        public string Image { get; set; }

        public Card (int Value, string Suit, string Image)
        {
            this.Value = Value;
            this.Suit = Suit;
            this.Image = Image;
        }
    }   

}