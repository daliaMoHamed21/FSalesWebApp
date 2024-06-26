using Core.Entities;
using Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesWebApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemService _itemService;
        private readonly CategoryService _categoryService;

        public ItemController(ItemService itemService, CategoryService categoryService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        // GET: Item
        public ActionResult Index()
        {
            var items = _itemService.GetAllItems();
            return View(items);
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _itemService.AddItem(item);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _itemService.UpdateItem(item);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _itemService.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}