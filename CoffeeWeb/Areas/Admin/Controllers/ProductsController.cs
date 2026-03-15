using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CoffeeWeb.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index(string searchText, int? page)
        {
            int pageSize = 10;
            page = page ?? 1;

            IEnumerable<Product> items = db.Products.OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Title.Contains(searchText) ||
                                         x.Alias.Contains(searchText));
            }

            int pageIndex = page.Value;
            items = items.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategoryList = db.ProductCategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategoryList = db.ProductCategories.ToList();
                return View(model);
            }

            model.CreateDate = DateTime.Now;
            model.ModifiedDate = DateTime.Now;

            if (string.IsNullOrEmpty(model.Alias))
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);

            var nextId = db.Database.SqlQuery<int>("SELECT SEQ_PRODUCT_ID.NEXTVAL FROM DUAL").Single();
            model.Id = nextId;
            string oneImage = null;
            if (Images != null && Images.Count > 0)
            {
                int defaultIndex = (rDefault != null && rDefault.Count > 0) ? rDefault[0] : 1;
                int idx = Math.Max(1, Math.Min(defaultIndex, Images.Count)) - 1; // clamp
                oneImage = Images[idx];
            }

         
            model.Image = oneImage;
            if (!string.IsNullOrEmpty(oneImage))
            {
                model.ProductImage = model.ProductImage ?? new List<ProductImage>();
                model.ProductImage.Add(new ProductImage
                {
                    ProductId = model.Id,
                    Image = oneImage,
                    IsDefault = true
                });
            }

            db.Products.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            var item = db.Products
                         .Include(x => x.ProductImage)
                         .FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                if (item.ProductImage != null && item.ProductImage.Any())
                {
                    db.ProductImages.RemoveRange(item.ProductImage);
                }

                db.Products.Remove(item);
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);

            ViewBag.ProductCategoryList = new SelectList(
                db.ProductCategories.ToList(),
                "Id",
                "Title",
                product.ProductCategoryId
            );

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, List<string> Images, List<int> rDefault)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategoryList = new SelectList(db.ProductCategories.ToList(), "Id", "Title", model.ProductCategoryId);
                return View(model);
            }

            var product = db.Products.Include("ProductImage").FirstOrDefault(x => x.Id == model.Id);
            if (product == null) return HttpNotFound();

            product.Title = model.Title;
            product.Price = model.Price;
            product.PriceSale = model.PriceSale;
            product.Quantity = model.Quantity;
            product.OriginalPrice = model.OriginalPrice;
            product.ProductCategoryId = model.ProductCategoryId;
            product.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);
            product.ModifiedDate = DateTime.Now;

            // CHỈ LẤY 1 ẢNH
            string oneImage = null;
            if (Images != null && Images.Count > 0)
            {
                int defaultIndex = (rDefault != null && rDefault.Count > 0) ? rDefault[0] : 1;
                int idx = Math.Max(1, Math.Min(defaultIndex, Images.Count)) - 1; // clamp
                oneImage = Images[idx];
            }

            // update ảnh cột Product.Image
            if (!string.IsNullOrEmpty(oneImage))
                product.Image = oneImage;

            // nếu bạn có bảng ProductImage: xóa hết cũ, thêm lại đúng 1 cái
            var oldImages = db.ProductImages.Where(x => x.ProductId == product.Id).ToList();
            if (oldImages.Any())
                db.ProductImages.RemoveRange(oldImages);

            if (!string.IsNullOrEmpty(oneImage))
            {
                db.ProductImages.Add(new ProductImage
                {
                    ProductId = product.Id,
                    Image = oneImage,
                    IsDefault = true
                });
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Json(new { success = false });

            var idList = ids.Split(',').Select(int.Parse).ToList();

            var products = db.Products
                             .Include(x => x.ProductImage)
                             .Where(x => idList.Contains(x.Id))
                             .ToList();

            foreach (var product in products)
            {
                if (product.ProductImage != null && product.ProductImage.Any())
                {
                    db.ProductImages.RemoveRange(product.ProductImage);
                }

                db.Products.Remove(product);
            }

            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(true);
        }

        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(true);
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(true);
        }
    }
}
