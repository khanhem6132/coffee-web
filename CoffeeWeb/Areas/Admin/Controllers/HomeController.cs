using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;

namespace CoffeeWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //[AdminAuthorize(Roles = "Admin")]
        // GET: Admin/Home
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.OrderBy(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        [HttpGet]
        public ActionResult GetStatistics(string fromDate, string toDate)
        {
            try
            {
                var query = from o in db.Orders
                            join od in db.OrderDetails on o.Id equals od.OrderId
                            join p in db.Products on od.ProductId equals p.Id
                            select new
                            {
                                CreatedDate = o.CreateDate,
                                Quantity = od.Quantity,
                                Price = od.Price,
                                OriginalPrice = p.OriginalPrice
                            };

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(x => DbFunctions.TruncateTime(x.CreatedDate) >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(x => DbFunctions.TruncateTime(x.CreatedDate) <= endDate);
                }


                var result = query
                    .GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate))
                    .Select(x => new
                    {
                        Date = x.Key.Value,
                        TotalBuy = x.Sum(y => (long)y.Quantity * (long)y.OriginalPrice),
                        TotalSell = x.Sum(y => (long)y.Quantity * (long)y.Price)
                    })
                    .OrderBy(x => x.Date)
                    .ToList()
                    .Select(x => new
                    {
                        Date = x.Date.ToString("dd/MM/yyyy"),
                        DoanhThu = x.TotalSell,
                        LoiNhuan = x.TotalSell - x.TotalBuy
                    });

                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
