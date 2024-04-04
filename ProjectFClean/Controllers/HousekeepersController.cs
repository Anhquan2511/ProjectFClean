using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectFClean.Models;

namespace ProjectFClean.Controllers
{
    public class HousekeepersController : Controller
    {
        private ProjectFCleanEntities2 db = new ProjectFCleanEntities2();

        // GET: Housekeepers
        public ActionResult Index()
        {
            var housekeeper = db.Housekeepers.Include(h => h.Account);
            return View(housekeeper.ToList());
        }

        // GET: Housekeepers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Housekeeper housekeeper = db.Housekeepers.Find(id);
            if (housekeeper == null)
            {
                return HttpNotFound();
            }
            return View(housekeeper);
        }

        // GET: Housekeepers/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Name");
            return View();
        }

        // POST: Housekeepers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HID,Age,Gender,Price,Skill,Experiment,Description,Money,Address,AccountID")] Housekeeper housekeeper)
        {
            if (ModelState.IsValid)
            {
                db.Housekeepers.Add(housekeeper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Name", housekeeper.AccountID);
            return View(housekeeper);
        }


        [HttpPost, ActionName("UpdateProfile")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(FormCollection form)
        {

            // Extracting values from the form
            // Form chứa một khối các data => cách lấy data của từng đối tượng là form["tên đối tượng] nhưng dạng nguyên bảng của nó ( kiểu dữ liệu ) -> string
            var hidStr = form["Id"];
            var ageStr = form["Age"];
            var priceStr = form["Price"];

            int hid;
            int age;
            int price;
            // Vì nguyên bản là string nên tuổi nó đang là string -> cần parse qua int => khi parse sẽ có khả năng lỗi ( ví dụ a -> Int.parse(a) -> lỗi nên cần check lỗi như dưới
            if (!int.TryParse(hidStr, out hid) || !int.TryParse(ageStr, out age))
            {
                return RedirectToAction("Error");
            }

            if (!int.TryParse(priceStr, out price))
            {
                price = 0;
            }

            var gender = form["Gender"];
            var skill = form["Skill"];
            var experiment = form["Experiment"];
            var description = form["Description"];
            var address = form["Address"];

            var houseKeeper = db.Housekeepers.FirstOrDefault(p => p.HID == hid);

            if (houseKeeper != null)
            {
                houseKeeper.Age = age;
                houseKeeper.Price = price;
                houseKeeper.Gender = gender;
                houseKeeper.Skill = skill;
                houseKeeper.Experiment = experiment;
                houseKeeper.Description = description;
                houseKeeper.Address = address;
                db.Entry(houseKeeper).State = EntityState.Modified;
                Session["Housekeeper"] = houseKeeper;
            }
            else
            {
                houseKeeper = new Housekeeper()
                {
                    Age = age,
                    Price = price,
                    Gender = gender,
                    Skill = skill,
                    Experiment = experiment,
                    Description = description,
                    Address = address
                };
                Session["Housekeeper"] = houseKeeper;
                db.Housekeepers.Add(houseKeeper);
            }

            db.SaveChanges();
            return RedirectToAction("Index");


        }

        public ActionResult UpdateAccount(FormCollection form)
        {
            var idStr = form["Id"];
            var name = form["Name"];
            var email = form["Email"];
            var phone = form["Phone"];

            int id;

            if (!int.TryParse(idStr, out id))
            {
                return RedirectToAction("Error");
            }
            var account = db.Accounts.FirstOrDefault(p => p.AccountID == id);

            if (account != null)
            {
                account.Email = email;
                account.Phone = phone;
                account.Name = name;

                db.Entry(account).State = EntityState.Modified;
                Session["Account"] = account;
            }

            db.SaveChanges();
            return RedirectToAction("Index");



        }

        // GET: Housekeepers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Housekeeper housekeeper = db.Housekeepers.Find(id);
            if (housekeeper == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Name", housekeeper.AccountID);
            return View(housekeeper);
        }

        // POST: Housekeepers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HID,Age,Gender,Price,Skill,Experiment,Description,Money,Address,AccountID")] Housekeeper housekeeper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(housekeeper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Name", housekeeper.AccountID);
            return View(housekeeper);
        }

        // GET: Housekeepers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Housekeeper housekeeper = db.Housekeepers.Find(id);
            if (housekeeper == null)
            {
                return HttpNotFound();
            }
            return View(housekeeper);
        }

        // POST: Housekeepers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Housekeeper housekeeper = db.Housekeepers.Find(id);
            db.Housekeepers.Remove(housekeeper);
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





        // POST: Housekeepers/Delete/5
        [HttpPost, ActionName("ChangeAvt")]
        public ActionResult ChangeAvt(HttpPostedFileBase newAvatar) 
        {
            if (newAvatar != null)
            {
                // Đường dẫn đến thư mục imgH
                string imgHPath = Server.MapPath("~/img/imgH/");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(imgHPath))
                {
                    Directory.CreateDirectory(imgHPath);
                }
                var account = Session["Account"] as ProjectFClean.Models.Account;

                string fileName = account.Name ;

                string fileExtension = Path.GetExtension(newAvatar.FileName);

                string filePath = Path.Combine(imgHPath, fileName + fileExtension);

                using (FileStream fileToSave = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    newAvatar.InputStream.CopyTo(fileToSave);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
