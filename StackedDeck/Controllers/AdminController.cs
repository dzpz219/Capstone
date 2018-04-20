using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StackedDeck.Models;
using StackedDeck.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace StackedDeck.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Admin()
        {
            ViewBag.Message = "Admin Management";
            
            return View(modelData("Users"));
        }
        // POST: Update User
        [HttpPost]
        public ActionResult UpdateUser(string Username, string Firstname, string Lastname, string RecoveryQuestion1, string RecoveryAnswer1, string Email, string Phone, string Postal, string Address, string Province, string Country)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.FindByName(Username);
            user.Firstname = Firstname;
            user.Lastname = Lastname;
            user.RecoveryQuestion1 = RecoveryQuestion1;
            user.RecoveryAnswer1 = RecoveryAnswer1;
            user.Email = Email;
            user.Phone = Phone;
            user.Postal = Postal;
            user.Address = Address;
            user.Country = Country;
            user.Province = Province;
            UserManager.Update(user);

            return View("Admin", modelData("Users"));
        }

        // POST: Max Users
        // get the maximum number of users for pagination
        [HttpPost]
        public int getMaxUsers(string search)
        {
            string Search = (search == null) ? "" : search.ToLowerInvariant();
            string maxQuery = "SELECT COUNT(*) FROM dbo.AspNetUsers WHERE Username LIKE '%" + Search + "%' OR Email LIKE '%" + Search + "%'";
            IEnumerable<int> result = context.Database.SqlQuery<int>(maxQuery);
            int max = result.ElementAt(0);
            return (max % 20 != 0) ? max / 20 + 1 : max / 20;
        }

        // POST: Search
        // search database for users that have the matching search string in Username or Email
        [HttpPost]
        public ActionResult SearchUsers(string search, Nullable<int> page)
        {
            string Search = (search == null) ? "" : search.ToLowerInvariant();
            int Page = (page == null) ? 1 : (int)page;

            var viewModel = new AdminData();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var username = UserManager.Users.Where(i => i.UserName.Contains(search));
            var email = UserManager.Users.Where(i => i.Email.Contains(search));

            // merge results and return 20 results per page
            var allUsers = username.Union(email).Where(i => i.Role != "Admin").OrderBy(i => i.UserName).Skip((Page - 1) * 20).Take(20);
            viewModel.Users = allUsers;
            return PartialView("usersPartial", viewModel);
        }

        // POST: Ban User
        // toggle ban status of a user
        [HttpPost]
        public ActionResult BanUser(string toggleUser)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            string query = "UPDATE dbo.AspNetUsers SET LockoutEnabled=~LockoutEnabled WHERE UserName = '" + toggleUser + "'";
            context.Database.ExecuteSqlCommand(query);
            return View("Admin", modelData("Users"));
        }

        // POST: Upload ad
        [HttpPost]
        public ActionResult Upload(string adName)
        {
            foreach (string file in HttpContext.Request.Files)
            {
                var postedFile = Request.Files[file];
                postedFile.SaveAs(Server.MapPath("~/imgs/ads/") + adName + Path.GetExtension(postedFile.FileName));
            }
            return PartialView("adsPartial", modelData("Ads"));
        }

        // POST: Delete Ad
        [HttpPost]
        public ActionResult DeleteAd(string deleteFile)
        {
            string inactive = "~/imgs/ads/inactive/";

            // create an inactive folder if it doesn't exist
            if (!Directory.Exists(Server.MapPath(inactive)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(inactive));
            }

            // move file into the inactive file
            else
            {
                var path = Server.MapPath("~/imgs/ads/") + deleteFile;
                var newPath = Server.MapPath("~/imgs/ads/inactive/") + deleteFile;
                System.IO.File.Copy(path, newPath, true);
                System.IO.File.Delete(path);
            }
            return PartialView("adsPartial", modelData("Ads"));
        }

        // POST: DeleteReport
        [HttpPost]
        public ActionResult DeleteReport(string id)
        {
            string query = "DELETE FROM dbo.AspNetReportedUsers WHERE Id = '"+ id + "'";
            context.Database.ExecuteSqlCommand(query);
            return PartialView("reportsPartial", modelData("Reports"));
        }

        // POST: ApproveCredit
        [HttpPost]
        public ActionResult ApproveCredit(string id, string username, int amount)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.FindByName(username);
            user.Credits += amount;
            UserManager.Update(user);
            string approve = "UPDATE dbo.AspNetCreditRequests SET Approved=~Approved WHERE Id = '" + id + "'";
            context.Database.ExecuteSqlCommand(approve);

            return Partials("creditsPartial", "Credits");
        }

        [HttpPost]
        public ActionResult Partials(string V, string D)
        {
            return PartialView(V, modelData(D));
        }

        // populate the admin viewmodel with default based on request
        public AdminData modelData(string Data) {

            var viewModel = new AdminData();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            viewModel.CountryList = Country.CountryList();
            viewModel.ProvinceList = Country.ProvinceList();

            switch (Data)
            {
                case "Users":
                    var allUsers = UserManager.Users.Where(i => i.Role == "User").OrderBy(i => i.UserName).Take(20);
                    viewModel.Users = allUsers;
                    return viewModel;
                case "Ads":
                    var allAds = new List<UploadedFile>();
                    var files = Directory.GetFiles(Server.MapPath("~/imgs/ads/"));
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        var uploadedFile = new UploadedFile();
                        uploadedFile.Name = Path.GetFileNameWithoutExtension(file);
                        uploadedFile.FullName = Path.GetFileName(file);
                        uploadedFile.Size = fileInfo.Length;
                        uploadedFile.Path = ("/imgs/ads/") + Path.GetFileName(file);
                        allAds.Add(uploadedFile);
                    }
                    viewModel.Ads = allAds;
                    return viewModel;
                case "Credits":
                    string query = "SELECT * FROM dbo.AspNetCreditRequests ORDER BY RequestDate DESC";
                    IEnumerable<CreditRequest> requests = context.Database.SqlQuery<CreditRequest>(query);
                    viewModel.Requests = requests;
                    return viewModel;

                case "Reports":
                    string queryreports = "SELECT dbo.AspNetReportedUsers.Id, Username, Content, PostDate, Avatar, ReportBy, ReportDate FROM dbo.AspNetDiscussionPosts JOIN dbo.AspNetReportedUsers ON dbo.AspNetDiscussionPosts.Id = dbo.AspNetReportedUsers.PostId ORDER BY ReportDate";
                    IEnumerable<ReportedUser> reports = context.Database.SqlQuery<ReportedUser>(queryreports);
                    viewModel.Reports = reports;
                    return viewModel;
            }
            
            return viewModel;
        }
    }
}