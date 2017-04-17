using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DemoRestaurant.DAL;
using DemoRestaurant.Models;
using System.Web.Mvc;

namespace DemoRestaurant.Models
{
    public class ShoppingCart
    {
        RestaurantDemoContext ResDB = new RestaurantDemoContext();
        String ShoppingCartId { get; set; }
        public const string CartSessionKey = "CardId";
        public static ShoppingCart GetCart(HttpContextBase Context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(Context);
            return cart;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        //  Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller Controller)
        {
            return GetCart(Controller.HttpContext);
        }

        public void AddToCart(Product Product)
        {
            
            // Get the matching cart and Product instances
            var cartItem = ResDB.Cart.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == Product.ProductId);
            if(cartItem==null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductId = Product.ProductId,
                    CartId = ShoppingCartId,
                    ProductQuantity = 1,
                    DateCreated = DateTime.Now

                };
                ResDB.Cart.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.ProductQuantity++;
            }
            ResDB.SaveChanges();
            
           
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = ResDB.Cart.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.ProductQuantity > 1)
                {
                    cartItem.ProductQuantity--;
                    itemCount = cartItem.ProductQuantity;
                }
                else
                {
                    ResDB.Cart.Remove(cartItem);
                }
                // Save changes
                ResDB.SaveChanges();
            }
            return itemCount;

        }
        public void EmptyCart()
        {
            var cartItems = ResDB.Cart.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                ResDB.Cart.Remove(cartItem);
            }
            // Save changes
            ResDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return ResDB.Cart.Where( cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in ResDB.Cart
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.ProductQuantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in ResDB.Cart
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.ProductQuantity *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)

            {
                //Cần fix thêm orderdetail
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    ProductQuantity = item.ProductQuantity,
                    // cần add product discount
                    Total = item.ProductQuantity * item.Product.Price,
                };
                // kiểm tra kho hàng còn hàng không sơ bộ, cần fix
                int? countSold = (from s in ResDB.OrderDetail
                                  where s.ProductId == item.ProductId
                                  && s.Order.CreatedAt == DateTime.Today
                                  select (int?)s.ProductQuantity).Sum();
                if (countSold == null) countSold = 0;
                if (orderDetail.ProductQuantity > item.Product.MaximunQuantity - countSold)
                    return 0;
                // lưu orderdetail
                item.Product.TotalSold += item.ProductQuantity;
                ResDB.OrderDetail.Add(orderDetail);

            }

            // lưu order
            ResDB.Order.Add(order);
            //lưu vào database
            ResDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = ResDB.Cart.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            ResDB.SaveChanges();
        }

    }
}
