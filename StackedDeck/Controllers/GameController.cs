using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackedDeck.Models;
using StackedDeck.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StackedDeck.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public static Stack<Card> Cards = Deck.NewDeck();
        public static Stack<Card> DealerHand = new Stack<Card>();
        public static Stack<Card> PlayerHand = new Stack<Card>();
        public static Card FaceDownCard;
        public static Double DealerHandValue;
        public static Double PlayerHandValue;
        public static Double BetAmount;

        public ActionResult Play()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());

            if (user.Credits < 1)
            {
                return RedirectToAction("Credit", "Home");
            }

            PlayViewModel viewModel = new PlayViewModel();
            viewModel.Credits = Math.Round(user.Credits, 2);
            return View(viewModel);
        }

        // New Game
        public void NewGame()
        {
            Cards.Clear();
            DealerHand.Clear();
            PlayerHand.Clear();
            DealerHandValue = 0;
            PlayerHandValue = 0;
            Cards = Deck.NewDeck();
        }

        // Deal hand
        [HttpPost]
        public ActionResult DealHand(double Bet)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());
            user.Credits -= Math.Round(Bet, 2);
            BetAmount = Math.Round(Bet, 2);
            UserManager.Update(user); // remove bet amount from user credits
            FaceDownCard = Cards.Pop();
            CardViewModel viewModel = new CardViewModel();
            DealerHand.Push(Cards.Pop());
            DealerHand.Push(new Card(FaceDownCard.Value, "", "card_back.png"));
            PlayerHand.Push(Cards.Pop());
            PlayerHand.Push(Cards.Pop());
            viewModel.DealerHand = DealerHand;
            viewModel.PlayerHand = PlayerHand;
            ViewBag.Ace = false;

            foreach (var Card in DealerHand)
            {
                DealerHandValue += Card.Value;
            }

            foreach (var Card in PlayerHand)
            {
                if (Card.Value == 1)
                {
                    ViewBag.Ace = true;
                    ViewBag.Soft = true;
                }
                PlayerHandValue += Card.Value;
            }
            ViewBag.Hand = PlayerHandValue;
            return PartialView("handPartial", viewModel);
        }

        // Get card
        [HttpGet]
        public ActionResult Hit(Nullable<bool> Soft)
        {
            CardViewModel viewModel = new CardViewModel();
            PlayerHandValue += Cards.Peek().Value;
            ViewBag.Ace = false;
            PlayerHand.Push(Cards.Pop());
            foreach (var Card in PlayerHand)
            {
                //checking for aces
                if (Card.Value == 1)
                {
                    ViewBag.Ace = true;
                    ViewBag.Soft = (Soft == null) ? true : Soft;
                }
            }
            viewModel.PlayerHand = PlayerHand;
            viewModel.DealerHand = DealerHand;
            ViewBag.Hand = PlayerHandValue;
            return PartialView("playerHandPartial", viewModel);
        }

        // Stand
        [HttpGet]
        public ActionResult Stand()
        {
            CardViewModel viewModel = new CardViewModel();
            DealerHand.Pop();
            DealerHand.Push(FaceDownCard);

            while (DealerHandValue < 17)
            {
                DealerHandValue += Cards.Peek().Value;
                DealerHand.Push(Cards.Pop());
            }
            viewModel.DealerHand = DealerHand;
            ViewBag.DealerHand = DealerHandValue;
            return PartialView("dealerHandPartial", viewModel);
        }

        // Toggle Ace Value
        [HttpPost]
        public ActionResult Ace(bool Soft)
        {
            CardViewModel viewModel = new CardViewModel();
            ViewBag.Hand = Soft ? PlayerHandValue += 10 : PlayerHandValue -= 10 ;
            viewModel.PlayerHand = PlayerHand;
            ViewBag.Soft = !Soft;
            ViewBag.Ace = true;
            return PartialView("playerHandPartial", viewModel);
        }

        // Hit Result
        public string HitResult()
        {
            if (PlayerHandValue > 21)
            {
                return "<div class='text-danger'>Lose</div>";
            }
            else
            {
                return "";
            }
        }

        // Get Result
        public string Result()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());

            if ((DealerHandValue <= 21 && PlayerHandValue < DealerHandValue) || (PlayerHandValue > 21 && DealerHandValue <= 21))
            {
                return "<div class='text-danger'>Lose</div>";
            }
            else if ((PlayerHandValue <= 21 && PlayerHandValue > DealerHandValue) || (DealerHandValue > 21 && PlayerHandValue <= 21))
            {
                user.Credits += BetAmount * 2;
                UserManager.Update(user);
                return "<div class='text-success'> Win " + (BetAmount * 2).ToString("C") + "</div>";
            }
            else
            {
                user.Credits += BetAmount;
                UserManager.Update(user);
                return "Tie";
            }
        }
    }
}