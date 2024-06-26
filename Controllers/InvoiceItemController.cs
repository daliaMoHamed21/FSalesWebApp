using Core.Entities;
using Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesWebApp.Controllers
{
    public class InvoiceItemController : Controller
    {
        private readonly InvoiceItemService _invoiceItemService;

        public InvoiceItemController(InvoiceItemService invoiceItemService)
        {
            _invoiceItemService = invoiceItemService;
        }

        // GET: InvoiceItem
        public ActionResult Index(int invoiceId)
        {
            var items = _invoiceItemService.GetInvoiceItems(invoiceId);
            return View(items);
        }

        // GET: InvoiceItem/Details/5
        public ActionResult Details(int id)
        {
            var item = _invoiceItemService.GetInvoiceItems(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: InvoiceItem/Create
        public ActionResult Create(int invoiceId)
        {
            ViewBag.InvoiceId = invoiceId;
            return View();
        }

        // POST: InvoiceItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceItem item)
        {
            if (ModelState.IsValid)
            {
                _invoiceItemService.Add(item);
                return RedirectToAction("Index", new { invoiceId = item.InvoiceId });
            }
            ViewBag.InvoiceId = item.InvoiceId;
            return View(item);
        }

        // GET: InvoiceItem/Edit/5
        public ActionResult Edit(int id)
        {
            var item = _invoiceItemService.GetInvoiceItems(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: InvoiceItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceItem item)
        {
            if (ModelState.IsValid)
            {
                _invoiceItemService.Update(item);
                return RedirectToAction("Index", new { invoiceId = item.InvoiceId });
            }
            return View(item);
        }

        // GET: InvoiceItem/Delete/5
        public ActionResult Delete(int id)
        {
            var item = _invoiceItemService.GetInvoiceItems(id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: InvoiceItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = _invoiceItemService.GetInvoiceItems(id).FirstOrDefault();
            if (item != null)
            {
                _invoiceItemService.Delete(item.Id);
                return RedirectToAction("Index", new { invoiceId = item.InvoiceId });
            }
            return HttpNotFound();
        }
    }
}