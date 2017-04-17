using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoRestaurant.Models;

namespace DemoRestaurant.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItem { get; set; }
        public Decimal CartTotal { get; set; }
    }
}