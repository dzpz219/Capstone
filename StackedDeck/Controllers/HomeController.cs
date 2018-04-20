using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StackedDeck.Models;
using StackedDeck.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackedDeck.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        //Send a message to admin
        public ActionResult ContactAdmin(string Message, string Name, string Phone, string Email)
        {
            string sender = Name + ", " + Email + " " + Phone;
            string query = "INSERT INTO dbo.AspNetMessages (Sender, Recipient, Content, MessageDate) " +
                               "VALUES('" + sender + "', 'admin', '" + Message + "', GETDATE())";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Contact");
        }

        [Authorize]
        public ActionResult Credit()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());
            ViewBag.Credits = user.Credits.ToString("C");
            return View();
        }

        [Authorize]
        [HttpPost]
        public void CreditRequest(string Amount)
        {
            string credit = "";

            //prevent front end html editing value and submitting
            switch (Amount)
            {
                case "5000":
                    credit = "5000";
                    break;
                case "10000":
                    credit = "10000";
                    break;
                case "15000":
                    credit = "15000";
                    break;
                case "25000":
                    credit = "25000";
                    break;
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());
            ViewBag.Credits = user.Credits;
            string query = "INSERT INTO dbo.AspNetCreditRequests (Username, Amount, RequestDate) VALUES ('" + User.Identity.GetUserName() + "', " + credit + ", GETDATE())";
            db.Database.ExecuteSqlCommand(query);
        }

        public ActionResult HowToPlay()
        {
            return View();
        }

        [Authorize]
        public ActionResult Home()
        {
            var viewModel = new HomeViewModel();
            var allAds = new List<UploadedFile>();
            string path = "~/imgs/ads/";

            //create a folder if it doesn't exist
            if (!Directory.Exists(Server.MapPath(path)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
            }

            //load images to homepage carousel
            else
            {
                var files = Directory.GetFiles(Server.MapPath("~/imgs/ads/"));
                int index = 1;
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);

                    var uploadedFile = new UploadedFile();
                    uploadedFile.Name = Path.GetFileNameWithoutExtension(file);
                    uploadedFile.Size = fileInfo.Length;
                    uploadedFile.Index = index;
                    uploadedFile.Path = ("/imgs/ads/") + Path.GetFileName(file);
                    allAds.Add(uploadedFile);
                    index++;
                }
            }
            viewModel.Ads = allAds;
            return View(viewModel);
        }
    }
}