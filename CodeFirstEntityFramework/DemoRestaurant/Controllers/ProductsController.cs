using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.DAL;
using DemoRestaurant.Models;
using DemoRestaurant.ViewModel;
using PagedList;

namespace DemoRestaurant.Controllers
{
    public class ProductsController : Controller
    {
        private RestaurantDemoContext db = new RestaurantDemoContext();

        // GET: Products
        public ViewResult Index(string category, string sortOrder, string searchString, int? page)
        {
            List<ProductViewModel> productsVM = new List<ProductViewModel>();
            System.Linq.IQueryable<Product> products;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.CateSortParm = sortOrder == "Cate" ? "Cate_desc" : "Cate";
            ViewBag.Category = category;
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;

            if (category != null)
            {
                var findParent = db.Category.Where(s => s.CategoryName == category).SingleOrDefault();
                if (findParent != null && findParent.ParentCategoryId == null)
                    products = db.Product.Where(s => (s.Category.CategoryName == category || s.Category.ParentCategoryId == findParent.CategoryId) );
                else
                    products = db.Product.Where(s => s.Category.CategoryName == category );
            }
            else
            {
                products = db.Product;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }
            var shopCart = ShoppingCart.GetCart(this.HttpContext);
            List<Cart> pItems = shopCart.GetCartItems();

            foreach (var item in products)
            {
                int quantity = 0;
                foreach (Cart c in pItems)
                {
                    if (c.ProductId == item.ProductId)
                    {
                        quantity = c.ProductQuantity;

                    }
                }
                int? countSold = (from s in db.OrderDetail where s.ProductId == item.ProductId && s.Order.CreatedAt == DateTime.Today select (int?)s.ProductQuantity).Sum();
                if (countSold == null) countSold = 0;

                productsVM.Add(new ProductViewModel { product = item, availabel = item.MaximunQuantity - (int)countSold - quantity });
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(productsVM.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Description,CategoryID,Price,Discount,MaximunQuantity,TotalSold")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Description,CategoryID,Price,Discount,MaximunQuantity,TotalSold")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
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
