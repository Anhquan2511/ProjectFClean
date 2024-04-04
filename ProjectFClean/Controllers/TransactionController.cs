using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectFClean.Controllers
{
    public class TransactionController : Controller
    {
        // GET: TransactionController
        // GET: Transaction
        ProjectFCleanEntities2 db = new ProjectFCleanEntities2();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Transaction(int id)
        //{
        //    if (id != null)
        //    {
        //        double total = 0;
        //        List<Compact> list = db.Compacts.Where(x => x.RID == id || x.HID == id).ToList(); //Get list tat ca trans voi RID hoac HID bang id
        //        foreach (Compact x in list) // duyet het mang
        //        {
        //            total += (double)db.Housekeepers.FirstOrDefault(c => c.HID == x.HID).Money * (int)(x.Work_Time); //Cong so tien * so ngay vao total
        //        }


        //        return View();
        //    }
        //}





    }
}
