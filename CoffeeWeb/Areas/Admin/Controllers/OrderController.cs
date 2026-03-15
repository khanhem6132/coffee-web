using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CoffeeWeb.Models.EF.Enum;

namespace CoffeeWeb.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var pageSize = 10;
            IEnumerable<Order> items = db.Orders.OrderBy(x => x.Id);
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }
        public ActionResult Partial_Product(int id)
        {
            var item = db.OrderDetails.Where(x => x.OrderId == id);
            return PartialView(item);
        }
        //[HttpPost]

        //public ActionResult UpdateTT(int id,int trangthai)
        //{
        //    var item = db.Orders.Find(id);
        //    if(item != null)
        //    {
        //        db.Orders.Attach(item);
        //        item.TypePayment = trangthai;
        //        db.Entry(item).Property(x => x.TypePayment).IsModified = true;
        //        db.SaveChanges();
        //        return Json(new { message = "Success", Success = true });
        //    }
        //    return Json(new { message = "UnSuccess", Success = false });

        //}
        [HttpPost]
        public JsonResult UpdateTT(int id, int trangthai)
        {
            var order = db.Orders.Find(id);
            if (order != null)
            {
                order.Status = (OrderStatus)trangthai;
                db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

    }
}