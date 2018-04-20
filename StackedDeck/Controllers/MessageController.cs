using StackedDeck.ViewModels;
using StackedDeck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace StackedDeck.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Conversations
        public ActionResult Messages()
        {
            MessageViewModel viewModel = new MessageViewModel();
            string query = "SELECT Sender, COUNT(*) AS Replies FROM dbo.AspNetMessages WHERE Recipient = '" + User.Identity.Name + "' GROUP BY Sender";
            IEnumerable<Message> Messages = db.Database.SqlQuery<Message>(query);
            viewModel.Messages = Messages;
            return View(viewModel);
        }

        // POST: User Conversation
        [HttpPost]
        public ActionResult UserConversation(string Sender)
        {
            MessageViewModel viewModel = GetUserConversation(Sender);
            return PartialView(viewModel);
        }

        // POST: Message Reload
        [HttpPost]
        public ActionResult MessageReload(string Sender)
        {
            MessageViewModel viewModel = GetUserConversation(Sender);
            return PartialView("ConversationPartial", viewModel);
        }

        // GET: User Conversation
        public MessageViewModel GetUserConversation(string Sender)
        {
            MessageViewModel viewModel = new MessageViewModel();
            ViewBag.Sender = Sender;
            string Recipient = User.Identity.Name;
            string query = "SELECT * FROM dbo.AspNetMessages WHERE (Recipient = '" + Recipient + "' AND Sender = '" + Sender + "') OR " +
                            "(Recipient = '" + Sender + "' AND Sender = '" + Recipient + "') ORDER BY MessageDate ASC";
            IEnumerable<Message> Messages = db.Database.SqlQuery<Message>(query);
            viewModel.Messages = Messages;
            return viewModel;
        }

        // POST: New Message
        [HttpPost]
        public async Task<ActionResult> NewMessage(string recipient, string message)
        {
            var user = await UserManager.FindByNameAsync(recipient);
            if (user != null)
            {
                string Sender = User.Identity.Name;
                string query = "INSERT INTO dbo.AspNetMessages (Sender, Recipient, Content, MessageDate) " +
                               "VALUES('" + Sender + "', '" + recipient + "', '" + message + "', GETDATE())";
                db.Database.ExecuteSqlCommand(query);
            }
            return PartialView("ConversationPartial", GetUserConversation(recipient));
        }

        // POST: Search Message
        [HttpPost]
        public ActionResult Search(string search, string mail)
        {
            MessageViewModel viewModel = new MessageViewModel();
            string s = search.ToLowerInvariant();
            string user = User.Identity.Name;
            string query = "SELECT Sender, COUNT(*) AS Replies FROM dbo.AspNetMessages WHERE Recipient = '" + user + "' AND " + "(Sender LIKE '%" + s + "%') GROUP BY Sender";
            IEnumerable<Message> Messages = db.Database.SqlQuery<Message>(query);

            viewModel.Messages = Messages;
            return PartialView("MessagesPartial", viewModel);
        }

    }
}