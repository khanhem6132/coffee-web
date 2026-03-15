using CoffeeWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Controllers
{
    [CustomerAuthorize]
    public class OrderHistoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Danh sách đơn hàng của người dùng hiện tại
        public ActionResult MyOrders()
        {
            //var userEmail = User.Identity.GetUserName(); // hoặc GetUserId() nếu bạn lưu theo Id

            //var orders = db.Orders
            //    .Where(o => o.Email == userEmail)
            //    .OrderByDescending(o => o.CreateDate)
            //    .ToList();


            //return View(orders);
            var userId = User.Identity.GetUserId();

            var orders = db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateDate)
                .ToList();

            return View(orders);
        }

        // Chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();

            var order = db.Orders
                .Include("OrderDetails")
                .Include("OrderDetails.Product")
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }


    }
}