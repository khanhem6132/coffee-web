using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeWeb.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ProductCategory> items = db.ProductCategories.OrderBy(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            var prcategories = db.ProductCategories.ToList();
            ViewBag.CategoryList = prcategories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int? id)
        {
            var productcategory = db.ProductCategories.Find(id);
            ViewBag.ProductcategoryList = productcategory;
            //ViewBag.CategoryList = db.Categories.ToList();
            //var item = db.News.Find(id);
            return View(productcategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);
                db.ProductCategories.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = db.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductCategories.Find(id);
            if (item != null)
            {

                db.ProductCategories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(false);
        }
        //[HttpPost]
        //public ActionResult IsActive(int id)
        //{
        //    var item = db.ProductCategories.Find(id);
        //    if (item != null)
        //    {
        //        item.IsActive = !item.IsActive;
        //        db.SaveChanges();
        //        return Json(new { success = true, isActive = item.IsActive });
        //    }
        //    return Json(true);
        //}
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.ProductCategories.Find(Convert.ToInt32(item));
                        db.ProductCategories.Remove(obj);
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}