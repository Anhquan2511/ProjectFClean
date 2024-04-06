using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectFClean.Controllers
{
    public class AccountsController : Controller
    {
        ProjectFCleanEntities6 db = new ProjectFCleanEntities6();



        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account, Renter renter, Housekeeper housekeeper)
        {
            if (CheckEmail(account.Email))
            {
                ModelState.AddModelError("", "Email already exists");
            }
            else
            {
                db.Accounts.Add(account);
                db.SaveChanges();

                var accId = account.AccountID;

                if (account.Role == "Renter")
                {
                    Renter renterAdd = new Renter
                    {
                        AccountID = accId,
                        Address = renter.Address,
                        Money = 0 // hoặc giá trị mặc định khác tùy thuộc vào yêu cầu của ứng dụng
                    };
                    db.Renters.Add(renterAdd);
                    db.SaveChanges();
                }
                else if (account.Role == "Housekeeper")
                {
                    Housekeeper housekeeperAdd = new Housekeeper
                    {
                        AccountID = accId,
                        Address = housekeeper.Address,
                        Age = housekeeper.Age,
                        Gender = housekeeper.Gender,
                        Price = housekeeper.Price,
                        Skill = housekeeper.Skill,
                        
                        Description = housekeeper.Description,
                        Money = 0, // hoặc giá trị mặc định khác tùy thuộc vào yêu cầu của ứng dụng  
                    };
                    db.Housekeepers.Add(housekeeperAdd);
                    db.SaveChanges();
                }
                TempData["SuccessMessage"] = "Registration successful. Please login.";
                return RedirectToAction("Login", "Accounts");
            }
            return View(account);
        }

        private bool CheckEmail(string email)
        {
            return db.Accounts.Any(x => x.Email == email);
        }

        public ActionResult Login()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(Account account)
        //{
        //    var Email = account.Email;
        //    var Password = account.Password;
        //    var CheckUser = db.Account.SingleOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));
        //    if (CheckUser != null)
        //    {
        //        // Kiểm tra role của người dùng
        //        string role = CheckUser.Role;
        //        if (role != null && (role == "Housekeeper" || role == "Renter"))
        //        {
        //            Session["Account"] = CheckUser;
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else if (role != null && role == "Admin")
        //        {
        //            Session["Account"] = CheckUser;
        //            return RedirectToAction("Index", "Admin");
        //        }
        //        else
        //        {
        //            // Trường hợp role không hợp lệ
        //            ViewBag.LoginFail = "Invalid role. Please contact administrator.";
        //            return View("Login");
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.LoginFail = "Login failed. Try again.";
        //        return View("Login");
        //    }
        //}
        [HttpPost]
        public ActionResult Login(Account account)
        {
            var Email = account.Email;
            var Password = account.Password;
            var CheckUser = db.Accounts.SingleOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));
            if (CheckUser != null)
            {
                // Kiểm tra trạng thái của người dùng
                string status = CheckUser.Approve;
                if (status != null)
                {
                    if (status == "Yes")
                    {
                        // Kiểm tra role của người dùng
                        string role = CheckUser.Role;
                        if (role != null && (role == "Housekeeper" || role == "Renter"))
                        {
                            Session["Account"] = CheckUser;
                            return RedirectToAction("Index", "Home");
                        }
                        else if (role != null && role == "Admin")
                        {
                            Session["Account"] = CheckUser;
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            // Trường hợp role không hợp lệ
                            ViewBag.LoginFail = "Invalid role. Please contact administrator.";
                            return View("Login");
                        }
                    }
                    else if (status == "No")
                    {
                        ViewBag.LoginFail = "Your account has not been approved.";
                        return View("Login");
                    }
                    else if (status == "Lock")
                    {
                        ViewBag.LoginFail = "Your account has been locked.";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.LoginFail = "Invalid account status.";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.LoginFail = "Login failed. Try again.";
                return View("Login");
            }
            ViewBag.LoginFail = "Login failed. Try again.";
            return View("Login");
        }
        public ActionResult AccountDetail(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

    }
}