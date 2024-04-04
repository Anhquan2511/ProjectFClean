using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectFClean.Controllers
{
    public class AdminController : Controller
    {
        ProjectFCleanEntities2 db = new ProjectFCleanEntities2();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // Action để xử lý tạo mới dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // Action để hiển thị form cập nhật
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return View();
            }

            var account = db.Accounts.Find(id);
            if (account == null)
            {
                return View();
            }

            return View(account);
        }

        // Action để xử lý cập nhật dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Account account)
        {
            if (id != account.AccountID)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // Action để hiển thị chi tiết tài khoản
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return View();
            }

            var account = db.Accounts.FirstOrDefault(m => m.AccountID == id);
            if (account == null)
            {
                return View();
            }

            return View(account);
        }

        // Action để xoá tài khoản
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return View();
            }

            var account = db.Accounts.FirstOrDefault(m => m.AccountID == id);
            if (account == null)
            {
                return View();
            }

            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}