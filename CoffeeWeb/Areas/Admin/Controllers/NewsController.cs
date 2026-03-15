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
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderBy(x => x.Id);
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
            var news = db.News.ToList();
            ViewBag.NewsList = news;
            var category = db.Categories.ToList();
            ViewBag.CategoryList = category;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {

                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NewsList = db.News.ToList();
            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            var news = db.News.ToList();
            ViewBag.NewsList = news;
            ViewBag.CategoryList = db.Categories.ToList();
            var item = db.News.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);
                db.News.Attach(model);
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
            var item = db.News.Find(id);
            if (item != null)
            {
                var DeleteItem = db.News.Attach(item);
                db.News.Remove(DeleteItem);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(true);
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(true);
        }
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
                        var obj = db.News.Find(Convert.ToInt32(item));
                        db.News.Remove(obj);
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}