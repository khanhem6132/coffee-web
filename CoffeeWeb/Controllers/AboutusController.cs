using CoffeeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Controllers
{
    public class AboutusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: News
        public ActionResult Index()
        {
            // Lấy 1 bài About Us đang active (ưu tiên mới nhất)
            var item = db.About_us
                         .Where(x => x.IsActive)
                         .OrderByDescending(x => x.Id)
                         .FirstOrDefault();

            return View(item);
        }
        public ActionResult Details(int id)
        {
            var model = db.About_us.Find(id);
            return View(model);
        }
        public ActionResult Partial_News_Home()
        {
            var item = db.About_us.Take(3).ToList();
            return PartialView(item);
        }
    }
}