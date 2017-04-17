using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoRestaurant.Models;

namespace DemoRestaurant.ViewModel
{
    public class ProductViewModel
    {
        public Product product;
        public int availabel { get; set; }
        public int inventory { get; set; }
    }
}