using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectFClean.Models;
using static ProjectFClean.Controllers.HomeController;

namespace ProjectFClean.Controllers
{
    public class PostsController : Controller
    {
        private readonly ProjectFCleanEntities2 db = new ProjectFCleanEntities2();

        public class PostIndexViewModel
        {
            public List<Post> ListPost { get; set; }
            public List<Service> ListService { get; set; }
        }


        // GET: Posts
        public ActionResult Index()
        {
            var viewModelPost = new PostIndexViewModel
            {
                ListPost = db.Posts.ToList(),

            };
            return View(viewModelPost);
            //var post = db.Post.Include(p => p.PID);            
            //var post = db.Post; // No Include statement
            //return View(post.ToList());

        }
        // GET: Posts/Details/5
        public ActionResult DetailsPost(int pid = 0)
        {

            if (pid == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(pid);
            if (post == null)
            {
                return HttpNotFound();
            }
            //return View(post);
            return View("Details", post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> serviceOptions = GetServiceOptions();
            ViewData["ServiceID"] = serviceOptions;
            return View();
        }

        private IEnumerable<SelectListItem> GetServiceOptions()
        {
            // Query the database for services
            var services = db.Services.ToList();

            // Create a list of SelectListItem objects
            var options = new List<SelectListItem>();

            // Add an option to allow for no selection
            options.Add(new SelectListItem { Value = "", Text = " Select a Service " });

            // Populate the options list with service data
            foreach (var service in services)
            {
                options.Add(new SelectListItem
                {
                    Value = service.ServiceID.ToString(),
                    Text = service.Name_of_service
                });
            }

            return options;
        }


        // POST: Posts/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PID,ServiceID,Price,Location,Gender,Age,Experience,Description,RID,HID,DatePost")] Post post)
        {
            if (ModelState.IsValid)
            {
                int maxPID = db.Posts.Max(p => (int?)p.PID) ?? 0;
                post.PID = maxPID + 1;

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name_of_service", post.ServiceID);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name_of_service", post.ServiceID);
            return View(post);
        }


        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PID,ServiceID,Price,Location,Gender,Age,Experience,Description,RID,HID,DatePost")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name_of_service", post.ServiceID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
