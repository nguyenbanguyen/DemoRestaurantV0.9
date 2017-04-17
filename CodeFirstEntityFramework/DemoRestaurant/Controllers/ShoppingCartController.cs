using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.Models;
using DemoRestaurant.ViewModel;
using DemoRestaurant.DAL;

namespace DemoRestaurant.Controllers
{
    public class ShoppingCartController : Controller
    {
        RestaurantDemoContext ResDb = new RestaurantDemoContext();
        // GET: ShoppingCart
        [HttpGet]
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItem = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
            
        }
        // POST:ShoppingCart
        //[HttpPost]
        //public ActionResult Index([Bind(inc)
        //{
        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    // Set up our ViewModel
        //    var viewModel = new ShoppingCartViewModel
        //    {
        //        CartItem = cart.GetCartItems(),
        //        CartTotal = cart.GetTotal()
        //    };
        //    // Return the view
        //    return View(viewModel);

        //}
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedProduct = ResDb.Product
                .Single(p  => p.ProductId ==id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main store page for more shopping
             return RedirectToAction("Index", "Products");
           
        }
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string ProductName = ResDb.Cart
                .Single( x => x.RecordId == id).Product.ProductName;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(ProductName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
            
        }
        // get: /SHoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}