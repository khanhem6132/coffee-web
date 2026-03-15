using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string searchText)
        {
            IEnumerable<Product> items = db.Products.OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }

            return View(items);
        }
        public ActionResult SoSanh(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return RedirectToAction("Index");
            }

            var idList = ids.Split(',').Select(int.Parse).ToList();


            var products = db.Products
                             .Where(p => idList.Contains(p.Id))
                             .ToList();

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Refresh()
        {
            var item = new StatisticalProcViewModel();

            // Online visitors
            ViewBag.VisitorsOnline = HttpContext.Application["visitors_online"];

            // Statistics
            item.Today = HttpContext.Application["Today"]?.ToString();
            item.Yesterday = HttpContext.Application["Yesterday"]?.ToString();
            item.ThisWeek = HttpContext.Application["ThisWeek"]?.ToString();
            item.LastWeek = HttpContext.Application["LastWeek"]?.ToString();
            item.ThisMonth = HttpContext.Application["ThisMonth"]?.ToString();
            item.LastMonth = HttpContext.Application["LastMonth"]?.ToString();
            item.AllTime = HttpContext.Application["AllTime"]?.ToString();

            return PartialView("Refresh", item);

        }
        public ActionResult TestStats()
        {
            var item = CoffeeWeb.Models.Common.StatisticalUser.StaProc();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Partial_Subscribe()
        {

            return PartialView();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Subscribe(Subscribe req)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Subscribes.Add(new Subscribe { Email = req.Email, CreateDate = DateTime.Now });
        //        db.SaveChanges();
        //        return Json(new { Success = true });
        //    }
        //    var errors = ModelState.Values.SelectMany(v => v.Errors)
        //                      .Select(e => e.ErrorMessage)
        //                      .ToList();
        //    return Json(new { Success = false, Errors = errors });
        //}
    }
}