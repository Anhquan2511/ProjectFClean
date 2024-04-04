using ProjectFClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectFClean.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectFCleanEntities2 db;
        public HomeController()
        {
            db = new ProjectFCleanEntities2();
        }
        public class HomeIndexViewModel
        {
            public List<Housekeeper> ListHousekeeper { get; set; }
            public List<Service> ListService { get; set; }
        }
        public ActionResult Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                ListHousekeeper = db.Housekeepers .ToList(),
                ListService = db.Services.ToList()
            };

            return View(viewModel);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Logout()
        {
            Session.Clear(); // Xóa tất cả các giá trị trong session
            Session.Abandon(); // Hủy session hiện tại
            return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
        }
       
        public ActionResult Details(int HID = 0)
        {
            Housekeeper housekeeper = db.Housekeepers.Find(HID);
            if (housekeeper == null)
            {
                return HttpNotFound();
            }
            return View("DetailsHouseKeeper", housekeeper);
        }

        [HttpGet]
        public PartialViewResult Search(string gender, string location, string name, string service)
        {
            // Lọc danh sách HouseKeeper dựa trên các tham số tìm kiếm
            IQueryable<Housekeeper> housekeepers = db.Housekeepers;

            if (!string.IsNullOrEmpty(gender))
            {
                housekeepers = housekeepers.Where(h => h.Gender == gender);
            }

            if (!string.IsNullOrEmpty(location))
            {
                housekeepers = housekeepers.Where(h => h.Address == location);
            }

            if (!string.IsNullOrEmpty(name))
            {
                housekeepers = housekeepers.Where(h => h.Account.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(service))
            {
                // Kiểm tra xem housekeeper có chứa kỹ năng service trong danh sách kỹ năng của mình hay không
                housekeepers = housekeepers.Where(h => h.Skill.Contains(service));
            }

            List<Housekeeper> searchResult = housekeepers.ToList();

            return PartialView("_HousekeeperList", searchResult);

        }



        public PartialViewResult Sort(int sortOption)
        {



            // Get the list of housekeepers from the database
            var housekeepers = db.Housekeepers.ToList();

            // Perform sorting based on sortOption
            switch (sortOption)
            {
                case 1: // Price Ascending
                    housekeepers = housekeepers.OrderBy(h => h.Price).ToList();
                    break;
                case 2: // Price Decreasing
                    housekeepers = housekeepers.OrderByDescending(h => h.Price).ToList();
                    break;
                case 3: // Experience Decreasing
                    housekeepers = housekeepers.OrderByDescending(h => h.Experiment).ToList();
                    break;
                default:
                    // Default sorting (if necessary)
                    break;
            }

            // Return the sorted list as a partial view
            return PartialView("_HousekeeperList", housekeepers);

        }
    




    }
}