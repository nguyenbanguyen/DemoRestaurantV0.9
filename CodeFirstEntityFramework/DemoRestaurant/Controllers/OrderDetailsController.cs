using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.DAL;
using DemoRestaurant.Models;

namespace DemoRestaurant.Controllers
{
    public class OrderDetailsController : Controller
    {
        private RestaurantDemoContext db = new RestaurantDemoContext();

        // GET: OrderDetails
        public async Task<ActionResult> Index()
        {
            var orderDetail = db.OrderDetail.Include(o => o.Order).Include(o => o.Product);
            return View(await orderDetail.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = await db.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username");
            ViewBag.ProductId = new SelectList(db.Product, "ProductId", "ProductName");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderDetailId,OrderId,ProductId,ProductQuantity,Total")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetail.Add(orderDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Product, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = await db.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Product, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderDetailId,OrderId,ProductId,ProductQuantity,Total")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "Username", orderDetail.OrderId);
            ViewBag.ProductId = new SelectList(db.Product, "ProductId", "ProductName", orderDetail.ProductId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = await db.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = await db.OrderDetail.FindAsync(id);
            db.OrderDetail.Remove(orderDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
