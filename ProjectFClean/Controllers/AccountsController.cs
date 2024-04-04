using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectFClean.Controllers
{
    public class AccountsController : Controller
    {
        ProjectFCleanEntities2 db = new ProjectFCleanEntities2();



        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
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
                    db.SaveChanges() ;
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
                            Experiment = housekeeper.Experiment,
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
        [HttpPost]
        public ActionResult Login(Account account)
        {
            var Email = account.Email;
            var Password = account.Password;
            var CheckUser = db.Accounts.SingleOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Password));
            if (CheckUser != null) 
            {
                // Kiểm tra role của người dùng
                string role = CheckUser.Role;
                if (role != null && role == "Housekeeper" )
                {
                    Session["Account"] = CheckUser;
                    var houseKeeper = db.Housekeepers.SingleOrDefault(hk => hk.AccountID  == CheckUser.AccountID);
                    Session["Housekeeper"] = houseKeeper;
                    return RedirectToAction("Index", "Home");
                }
                else if (role != null && role == "Renter")
                {
                    Session["Account"] = CheckUser;
                    var renter = db.Renters.SingleOrDefault(rt => rt.AccountID == CheckUser.AccountID);
                    Session["Renter"] = renter;
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
            else
            {
                ViewBag.LoginFail = "Login failed. Try again.";
                return View("Login");
            }
        }

    }
}