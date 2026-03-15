using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CoffeeWeb.Models.EF.Enum;

namespace CoffeeWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [CustomerAuthorize]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {

            return View();
        }
        public ActionResult Partial_Checkout()
        {
            return PartialView();
        }
        //[CustomerAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };

            if (!ModelState.IsValid) return Json(code);

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart == null || cart.Items == null || cart.Items.Count == 0) return Json(code);

            // ✅ LẤY ID TRƯỚC TỪ SEQUENCE (Oracle)
            int nextOrderId = db.Database.SqlQuery<int>("SELECT SEQ_ORDERS_ID.NEXTVAL FROM DUAL").Single();

            Order order = new Order();
            order.Id = nextOrderId;                       // ✅ gán Id trước
            order.CustomerName = req.CustomerName;
            order.UserId = User.Identity.GetUserId();
            order.Phone = req.Phone;
            order.Address = req.Address;
            order.Email = req.Email;
            //order.Quantity = req.Quantity;
            order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
            order.TypePayment = req.TypePayment;
            order.ModifiedDate = DateTime.Now;
            order.CreateDate = DateTime.Now;

            order.Status = order.TypePayment == 1
            ? OrderStatus.Wait_for_confirmation
            : OrderStatus.Confirmed;


            Random rd = new Random();
            order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

            // ✅ add order trước
            db.Orders.Add(order);

            // ✅ add details, PHẢI set OrderId = nextOrderId
            foreach (var x in cart.Items)
            {
                int nextDetailId = db.Database.SqlQuery<int>("SELECT SEQ_ORDERDETAIL_ID.NEXTVAL FROM DUAL").Single();

                db.OrderDetails.Add(new OrderDetail
                {
                    Id = nextDetailId,              
                    OrderId = nextOrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                });
            }


            db.SaveChanges();
            var strProduct = "";
            var Price = 0L;
            var TotalPrice = 0L;

            foreach (var item in cart.Items)
            {
                strProduct += "<tr>";
                strProduct += "<td>" + item.ProductName + "</td>";
                strProduct += "<td>" + item.Quantity + "</td>";
                strProduct += "<td>" + item.TotalPrice.ToString("#,##0") + "VND" + "</td>";
                strProduct += "</tr>";
                Price += item.Price * item.Quantity;
            }

            TotalPrice = Price;

            string connectCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send2.html"));
            connectCustomer = connectCustomer.Replace("{{MaDon}}", order.Code);
            connectCustomer = connectCustomer.Replace("{{SanPham}}", strProduct);
            connectCustomer = connectCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
            connectCustomer = connectCustomer.Replace("{{NgayDat}}", order.CreateDate.ToString());
            connectCustomer = connectCustomer.Replace("{{Phone}}", order.Phone);
            connectCustomer = connectCustomer.Replace("{{Email}}", req.Email);
            connectCustomer = connectCustomer.Replace("{{DiaChiKhachHang}}", order.Address);
            connectCustomer = connectCustomer.Replace("{{ThanhTien}}", Price.ToString("#,##0"));
            connectCustomer = connectCustomer.Replace("{{TongTien}}", TotalPrice.ToString("#,##0"));
            CoffeeWeb.Models.SendGmail.SendMail("ShopOnline", "Đơn hàng #" + order.Code, connectCustomer.ToString(), req.Email);

            string connectAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/send1.html"));
            connectAdmin = connectAdmin.Replace("{{MaDon}}", order.Code);
            connectAdmin = connectAdmin.Replace("{{SanPham}}", strProduct);
            connectAdmin = connectAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
            connectAdmin = connectAdmin.Replace("{{NgayDat}}", order.CreateDate.ToString());
            connectAdmin = connectAdmin.Replace("{{Phone}}", order.Phone);
            connectAdmin = connectAdmin.Replace("{{Email}}", req.Email);
            connectAdmin = connectAdmin.Replace("{{DiaChiKhachHang}}", order.Address);
            connectAdmin = connectAdmin.Replace("{{ThanhTien}}", Price.ToString("#,##0"));
            connectAdmin = connectAdmin.Replace("{{TongTien}}", TotalPrice.ToString("#,##0"));
            CoffeeWeb.Models.SendGmail.SendMail("ShopOnline", "Đơn hàng mới#" + order.Code, connectAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);

            cart.ClearCart();
            return RedirectToAction("CheckOutSuccess");
        }


        public ActionResult Partital_Item_Payment()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partital_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Success = true, count = cart.Items.Count }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { Success = false, Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };
            var db = new ApplicationDbContext();

            var checkProduct = db.Products
                                 .Include("ProductImage")
                                 .Include("ProductCategory")
                                 .FirstOrDefault(x => x.Id == id);

            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }

                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = quantity
                };



                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (int)checkProduct.PriceSale;
                }
                else
                {
                    item.Price = checkProduct.Price;
                }

                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;

                code = new { Success = true, msg = "Product added to cart successfully.", code = 1, count = cart.Items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Detele(int id)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeteleAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                {
                    cart.ClearCart();
                    return Json(new { Success = true });
                }

            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var code = new { Success = false, code = -1, count = 0, totalItem = 0L, totalCart = 0L };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                Session["Cart"] = cart;

                var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

                code = new
                {
                    Success = true,
                    code = 1,
                    count = cart.Items.Count,
                    totalItem = item != null ? item.TotalPrice : 0L,
                    totalCart = cart.GetTotal()   // long
                };
            }
            return Json(code);
        }


    }
}