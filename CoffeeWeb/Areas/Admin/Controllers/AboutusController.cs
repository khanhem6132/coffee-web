using CoffeeWeb.Models;
using CoffeeWeb.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace CoffeeWeb.Areas.Admin.Controllers
{
    public class AboutusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/About_us
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null) page = 1;

            // ✅ đúng: About_us
            IQueryable<Aboutus> query = db.About_us.Include(x => x.Category).OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var items = query.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.CategoryList = db.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Aboutus model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);

                db.About_us.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = db.Categories.ToList();
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.CategoryList = db.Categories.ToList();
            var item = db.About_us.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aboutus model)
        {
            if (ModelState.IsValid)
            {
                // ⚠️ CreateDate không nên set lại, nhưng mình giữ theo style của bạn
                model.ModifiedDate = DateTime.Now;
                model.Alias = CoffeeWeb.Models.Common.Filter.FilterChar(model.Title);

                db.About_us.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = db.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.About_us.Find(id);
            if (item != null)
            {
                var deleteItem = db.About_us.Attach(item);
                db.About_us.Remove(deleteItem);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.About_us.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
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
                        var obj = db.About_us.Find(Convert.ToInt32(item));
                        if (obj != null)
                        {
                            db.About_us.Remove(obj);
                        }
                    }
                    db.SaveChanges(); // ✅ save 1 lần
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
