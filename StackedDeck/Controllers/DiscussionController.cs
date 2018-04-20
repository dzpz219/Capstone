using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StackedDeck.Models;
using StackedDeck.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StackedDeck.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Discussion
        public ActionResult Discussion()
        {
            ViewBag.Message = "Discussions";
            return View(modelData());
        }

        // GET: ThreadsPartial
        public ActionResult ThreadsPartial()
        {
            return PartialView(modelData());
        }

        // POST: New Thread
        // insert a new thread with initial post
        [HttpPost]
        public void NewThread()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());

            string Avatar = (user.Avatar == null) ? "/imgs/person.png" : "/imgs/portraits/" + User.Identity.GetUserName() + "/" + User.Identity.GetUserName() + user.Avatar;
            string Topic = Request.Form["Thread.Topic"];
            string Content = Request.Form["Thread.Content"];
            string Author = User.Identity.GetUserName();
            string newThread = "INSERT INTO dbo.AspNetDiscussionThreads (Author, Topic, Content, PostDate) " +
                "VALUES('" + Author + "', '" + Topic + "', '" + Content + "', GETDATE())";
            db.Database.ExecuteSqlCommand(newThread);

            int ThreadId = db.Database.SqlQuery<int>("SELECT MAX(Id) FROM dbo.AspNetDiscussionThreads").ElementAt(0);
            string newPost = "INSERT INTO dbo.AspNetDiscussionPosts (ThreadId, Username, Content, PostDate, Avatar) " +
                "VALUES(" + ThreadId + ", '" + Author + "', '" + Content + "', GETDATE(), '" + Avatar + "')";
            db.Database.ExecuteSqlCommand(newPost);
        }

        // POST: Delete Thread
        // delete thread and all the posts in the thread
        [HttpPost]
        public ActionResult DeleteThread(int ThreadId)
        {
            string deleteThread = "DELETE FROM dbo.AspNetDiscussionThreads WHERE Id = '" + ThreadId + "'";
            string deletePostsInThread = "DELETE FROM dbo.AspNetDiscussionPosts WHERE ThreadId = '" + ThreadId + "'";

            db.Database.ExecuteSqlCommand(deleteThread);
            db.Database.ExecuteSqlCommand(deletePostsInThread);

            return View("Discussion", modelData());
        }

        // POST: Delete Post
        [HttpPost]
        public void DeletePost(int PostId)
        {
            string deletePost = "DELETE FROM dbo.AspNetDiscussionPosts WHERE Id = '" + PostId + "'";
            db.Database.ExecuteSqlCommand(deletePost);
        }

        // POST: Max Posts
        // for pagination
        [HttpPost]
        public int getMaxPosts(int ThreadId)
        {
            string maxQuery = "SELECT Count(*) FROM dbo.AspNetDiscussionPosts JOIN dbo.AspNetUsers ON dbo.AspNetDiscussionPosts.Username = dbo.AspNetUsers.UserName WHERE LockoutEnabled = 0 AND ThreadId = " + ThreadId;
            IEnumerable<int> result = db.Database.SqlQuery<int>(maxQuery);
            int max = result.ElementAt(0);
            return (max % 10 != 0) ? max / 10 + 1 : max / 10;
        }

        // POST: View Posts in a thread
        [HttpGet]
        public ActionResult Posts(int ThreadId, string Topic, string Author, Nullable<int> Page)
        {
            ViewBag.Title = Topic;
            ViewBag.ThreadId = ThreadId;
            ViewBag.Delete = (Author == User.Identity.Name.ToLowerInvariant() || User.IsInRole("Admin")) ? true : false;
            ViewBag.MaxPosts = getMaxPosts(ThreadId);
            ViewBag.Author = Author;

            int page = (Page == null) ? 1 : (int)Page;
            string view = (Page == null) ? "ThreadPosts" : "PostsPartial";
            var viewModel = new DiscussionViewModel();
            string query = "SELECT dbo.AspNetDiscussionPosts.Id, ThreadId, dbo.AspNetDiscussionPosts.Username, Content, dbo.AspNetDiscussionPosts.Avatar, PostDate FROM dbo.AspNetDiscussionPosts JOIN dbo.AspNetUsers ON dbo.AspNetDiscussionPosts.Username = dbo.AspNetUsers.UserName WHERE ThreadId = " + ThreadId + " AND LockoutEnabled = 0";
            IEnumerable<DiscussionPost> posts = db.Database.SqlQuery<DiscussionPost>(query).Skip((page - 1) * 10).Take(10);
            viewModel.Posts = posts;
            if (Page == null)
            {
                return View("ThreadPosts", viewModel);
            }
            else {
                return PartialView("PostsPartial", viewModel);
            }
        }

        // POST: New Post
        [HttpPost]
        public ActionResult NewPost(string Author, string ThreadId, string Content)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(User.Identity.GetUserName());            
            var viewModel = new DiscussionViewModel();

            ViewBag.Delete = User.IsInRole("Admin") ? true : false;
            string Avatar = (user.Avatar == null) ? "/imgs/person.png" : "/imgs/portraits/" + User.Identity.GetUserName() + "/" + User.Identity.GetUserName() + user.Avatar;
            string Username = User.Identity.GetUserName();
            string newPost = "INSERT INTO dbo.AspNetDiscussionPosts (ThreadId, Username, Content, PostDate, Avatar) " +
                "VALUES(" + ThreadId + ", '" + Username + "', '" + Content + "', GETDATE(), '" + Avatar + "')";
            db.Database.ExecuteSqlCommand(newPost);

            string query = "SELECT * FROM dbo.AspNetDiscussionPosts WHERE ThreadId = " + ThreadId;
            IEnumerable<DiscussionPost> posts = db.Database.SqlQuery<DiscussionPost>(query).Take(10);
            viewModel.Posts = posts;
            return PartialView("PostsPartial", viewModel);
        }

        // POST: Max Threads
        // for pagination
        [HttpPost]
        public int getMaxThreads(string search)
        {
            string Search = (search == null) ? "" : search.ToLowerInvariant();
            string maxQuery = "SELECT Count(*) FROM dbo.AspNetDiscussionThreads JOIN dbo.AspNetUsers ON dbo.AspNetDiscussionThreads.Author = dbo.AspNetUsers.UserName WHERE (Author LIKE '%" + Search + "%' OR Topic LIKE '%" + Search + "%')" + " AND LockoutEnabled = 0";
            IEnumerable <int> result = db.Database.SqlQuery<int>(maxQuery);
            int max = result.ElementAt(0);
            return (max % 20 != 0) ? max / 20 + 1 : max / 20;
        }

        // POST: Search Threads
        [HttpPost]
        public ActionResult SearchThreads(string search, Nullable<int> page)
        {
            string Search = (search == null) ? "" : search.ToLowerInvariant();
            int Page = (page == null) ? 1 : (int)page;
            var viewModel = new DiscussionViewModel();

            //Match threads author or topic with the search param. The user must not be banned
            string query = "SELECT dbo.AspNetUsers.LockoutEnabled, dbo.AspNetDiscussionThreads.Id, dbo.AspNetDiscussionThreads.Author, dbo.AspNetDiscussionThreads.Topic, dbo.AspNetDiscussionThreads.PostDate," +
                        "COUNT(dbo.AspNetDiscussionPosts.Id) AS PostCount FROM dbo.AspNetDiscussionThreads " +
                        "JOIN dbo.AspNetDiscussionPosts ON dbo.AspNetDiscussionThreads.Id = dbo.AspNetDiscussionPosts.ThreadId JOIN dbo.AspNetUsers ON dbo.AspNetDiscussionThreads.Author = dbo.AspNetUsers.UserName " +
                        "WHERE ThreadId = dbo.AspNetDiscussionThreads.Id AND ((Author LIKE '%" + Search + "%') OR (Topic LIKE '%" + Search + "%')) AND dbo.AspNetUsers.LockoutEnabled = 0 " +
                        "GROUP BY dbo.AspNetUsers.LockoutEnabled, dbo.AspNetDiscussionThreads.Id, dbo.AspNetDiscussionThreads.Author,dbo.AspNetDiscussionThreads.Topic,dbo.AspNetDiscussionThreads.PostDate " +
                        "ORDER By dbo.AspNetDiscussionThreads.Id DESC";
            IEnumerable<DiscussionThread> Results = db.Database.SqlQuery<DiscussionThread>(query).Skip((Page - 1) * 20).Take(20);
            viewModel.Threads = Results;

            return PartialView("ThreadsPartial", viewModel);
        }

        //POST: Report User
        [HttpPost]
        public void ReportPost(string PostId)
        {
            string query = "INSERT INTO dbo.AspNetReportedUsers (PostId, ReportBy, ReportDate) VALUES ('" + PostId + "', '" + User.Identity.GetUserName() + "', GETDATE())";
            db.Database.ExecuteSqlCommand(query);
        }

        public DiscussionViewModel modelData()
        {
            var viewModel = new DiscussionViewModel();
            //Select first 20 result of all the threads
            string query = "SELECT TOP 20 dbo.AspNetUsers.LockoutEnabled, dbo.AspNetDiscussionThreads.Id, dbo.AspNetDiscussionThreads.Author, dbo.AspNetDiscussionThreads.Topic, dbo.AspNetDiscussionThreads.PostDate," +
                        "COUNT(dbo.AspNetDiscussionPosts.Id) AS PostCount FROM dbo.AspNetDiscussionThreads " +
                        "JOIN dbo.AspNetDiscussionPosts ON dbo.AspNetDiscussionThreads.Id = dbo.AspNetDiscussionPosts.ThreadId JOIN dbo.AspNetUsers ON dbo.AspNetDiscussionThreads.Author = dbo.AspNetUsers.UserName " +
                        "WHERE ThreadId = dbo.AspNetDiscussionThreads.Id AND dbo.AspNetUsers.LockoutEnabled = 0 " +
                        "GROUP BY dbo.AspNetUsers.LockoutEnabled, dbo.AspNetDiscussionThreads.Id, dbo.AspNetDiscussionThreads.Author,dbo.AspNetDiscussionThreads.Topic,dbo.AspNetDiscussionThreads.PostDate " +
                        "ORDER By dbo.AspNetDiscussionThreads.Id DESC";
            IEnumerable<DiscussionThread> Threads = db.Database.SqlQuery<DiscussionThread>(query);
            viewModel.Threads = Threads;
            return viewModel;
        }
    }
}