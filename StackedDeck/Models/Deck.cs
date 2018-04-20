using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackedDeck.Models
{
    static class Deck
    {
        public enum Suit
        {
            Hearts,
            Diamonds,
            Spades,
            Clubs
        }

        public enum Image
        {
            ace_of_, 
            two_of_,
            three_of_,
            four_of_,
            five_of_,
            six_of_,
            seven_of_,
            eight_of_,
            nine_of_,
            ten_of_,
            jack_of_,
            queen_of_,
            king_of_
        }

        public static Stack<Card> NewDeck()
        {
            Stack<Card> d = new Stack<Card>();
            foreach (var Card in NewDeckList())
            {
                d.Push(Card);
            }
            return d;
        }

        public static List<Card> NewDeckList()
        {
            List<Card> d = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    d.Add(new Card(Math.Min(j + 1, 10), ((Suit)i).ToString(), ((Image)j).ToString() + ((Suit)i).ToString().ToLower() + ".png"));
                }
            }
            return Shuffle(d);
        }

        public static List<Card> Shuffle(List<Card> deck)
        {
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }
            return deck;
        }
    }
}