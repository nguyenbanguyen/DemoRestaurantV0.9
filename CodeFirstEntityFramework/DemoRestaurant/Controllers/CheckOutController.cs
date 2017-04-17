using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoRestaurant.DAL;
using DemoRestaurant.Models;
using System.Web.Security;
using System.Data.Entity.Validation;

namespace DemoRestaurant.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        RestaurantDemoContext ResDb = new RestaurantDemoContext();
       
        // GET: CheckOut/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            var Cart = ShoppingCart.GetCart(this.HttpContext);
            // kiểm tra xem cart có gì ko, nếu ko có thì cho về xem lại product, cần fix thêm
            if (Cart.GetCount() < 1)
            {
                return RedirectToAction("Index", "Products");
            }

            //Kiểm tra hàng còn trong kho không , cần fix thêm
            CheckOutViewModel check = new CheckOutViewModel();

            if (checkProductInCart() == false)
            {
                return RedirectToAction("Index", "Products");
            }

            check.ToTalPrice = Cart.GetTotal();


            //Need to finish customer /account controller soon
            //need to modified /add shipping date 
           
            //need to modified total price based on discount 
            check.CustomerPhone = ResDb.Customer.Where(s => s.CustomerName == User.Identity.Name).First().CustomerPhone;
            check.ShippingAddress = ResDb.Customer.Where(s => s.CustomerName == User.Identity.Name).First().ShippingAddress;
            check.ShippingDate = DateTime.Now;
            //// need to add discont later
            
            return View(check);

        }
        //check product con ko
        bool checkProductInCart()
        {
            var Cart = ShoppingCart.GetCart(this.HttpContext);
            var cartItems = Cart.GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                int? countSold = (from s in ResDb.OrderDetail where s.ProductId == item.ProductId && s.Order.CreatedAt == DateTime.Today select (int?)s.ProductQuantity).Sum();
                if (countSold == null) countSold = 0;
                if (item.ProductQuantity > item.Product.MaximunQuantity - countSold)
                    return false;
            }
            return true;
        }
        // giảm giá sơ bộ theo kiểu khách hàng---cần code thêm
        //int Discount(string UserName)
        //{
        //    string type = ResDb.Customer.Where(s => s.CustomerName == UserName).Single().CustomerType;
        //    if (type == "Vàng")
        //        return 10;
        //    else if (type == "Bạc")
        //        return 8;
        //    else if (type == "Đồng")
        //        return 5;
        //    else return 0;
        //}


        //fixbug, delete this later
        // GET: /Checkout/AddressAndPayment
        //public ActionResult AddressAndPayment()
        //{
        //    return View();
        //}

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment([Bind(Include = "CustomerPhone,ShippingAddress,ToTalPrice,ShippingDate")] CheckOutViewModel Checkout)
        {
            
            var order = new Order();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            //TryUpdateModel(order);

            try
            {
                   
                    order.Username = User.Identity.Name;
                    order.CreatedAt = DateTime.Now;
                    // need to modified shipping date more specific soon
                    order.ShippingDate = Checkout.ShippingDate;
                    order.TotalPrice =(int) cart.GetTotal();
                    order.ShippingAddress = Checkout.ShippingAddress;
                //if (string.IsNullOrEmpty( ResDb.Order.FirstOrDefault().ToString())) order.OrderId = 1;
                //else order.OrderId = ResDb.Order.Count();
                try
                {
                    order.OrderId = ResDb.Order.Count();
                }
                catch
                {
                    order.OrderId = 1;
                }

                // cần mã hóa orderid


                //Save Order
                try
                {
                    //ResDb.Order.Add(order);
                    //ResDb.SaveChanges();
                    
                    //Process the order
                    cart.CreateOrder(order);
                    cart.EmptyCart();
                }
                catch (DbEntityValidationException e)
                // kiểm tra lỗi khi add vào db
                {
                    string exceptiondb=null;
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        exceptiondb += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State.ToString() + " has the following validation errors:";
                       
                        foreach (var ve in eve.ValidationErrors)
                        {
                            exceptiondb += "  Property: " + ve.PropertyName + " Error:" + ve.ErrorMessage;
                           
                        }
                    }
                    ViewBag.exception = exceptiondb;
                    return View(Checkout);

                }
               




                // cần xử lý về tình trạng tài khoản của khách, etc trước khi thanh toán, sẽ làm sau

                    return RedirectToAction("Complete", new { id = order.OrderId });
                
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(Checkout);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            
            // Validate customer owns this order
            bool isValid = ResDb.Order.Any(
                o => o.OrderId == id && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }
}