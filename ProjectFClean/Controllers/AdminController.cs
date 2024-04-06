using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Web.DynamicData;
using System.Data.Entity.Validation;
using System.Runtime.Remoting.Messaging;


namespace ProjectFClean.Controllers
{
    public class AdminController : Controller
    {
        ProjectFCleanEntities6 db = new ProjectFCleanEntities6();
        // GET: Admin
        public List<Account> accounts = new List<Account>();

        // GET: Admin
        //    [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        //     [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Account account = db.Accounts.SingleOrDefault(n => n.AccountID == id);
            ViewBag.AccountID = account.AccountID;
            if (account == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(account);
        }
        //     [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            var renters = db.Renters.Where(r => r.AccountID == id);
            db.Renters.RemoveRange(renters);


            Account account = db.Accounts.SingleOrDefault(n => n.AccountID == id);
            if (account == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Accounts.Remove(account);
            db.SaveChanges();

            return RedirectToAction("User");
        }
        //    [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Account account = db.Accounts.SingleOrDefault(n => n.AccountID == id);
            if (account == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(account);
        }
        //    [Authorize(Roles = "Admin")]
        public ActionResult User(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 7;

            var approvedAccounts = db.Accounts.Where(a => a.Approve == "Yes").OrderBy(n => n.AccountID).ToPagedList(pageNumber, pageSize);
            return View("User", approvedAccounts);
        }
        //    [Authorize(Roles = "Admin")]
        public ActionResult Verification()
        {
            var accountsWithNoApprove = db.Accounts.Where(a => a.Approve == "No").OrderBy(n => n.AccountID).ToList();

            return View(accountsWithNoApprove);
        }
        //     [Authorize(Roles = "Admin")]
        public ActionResult VeriConfirm(int id = 0)
        {
            var account = db.Accounts.SingleOrDefault(n => n.AccountID == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            account.Approve = "Yes";
            db.SaveChanges();
            return RedirectToAction("Verification");
        }

        //      [Authorize(Roles = "Admin")]
        public ActionResult AccountDetail(int id = 0)
        {
            var account = db.Accounts.SingleOrDefault(n => n.AccountID == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            var renterDetail = db.Renters.FirstOrDefault(r => r.AccountID == id);
            var housekeeperDetail = db.Housekeepers.FirstOrDefault(h => h.AccountID == id);

            ViewBag.RenterDetail = renterDetail;
            ViewBag.HousekeeperDetail = housekeeperDetail;

            return View("AccountDetail", account);
        }
        //       [Authorize(Roles = "Admin")]
        public ActionResult LockAcc()
        {
            var accountsWithNoApprove = db.Accounts.Where(a => a.Approve == "Lock").OrderBy(n => n.AccountID).ToList();

            return View(accountsWithNoApprove);
        }
        //       [Authorize(Roles = "Admin")]
        public ActionResult Lock(int id = 0)
        {
            var account = db.Accounts.SingleOrDefault(n => n.AccountID == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            account.Approve = "Lock";
            db.SaveChanges();
            return RedirectToAction("User");
        }
        //        [Authorize(Roles = "Admin")]
        public ActionResult UnLock(int id = 0)
        {
            var account = db.Accounts.SingleOrDefault(n => n.AccountID == id);

            if (account == null)
            {
                return HttpNotFound();
            }

            account.Approve = "Yes";
            db.SaveChanges();
            return RedirectToAction("LockAcc");
        }
    }
}