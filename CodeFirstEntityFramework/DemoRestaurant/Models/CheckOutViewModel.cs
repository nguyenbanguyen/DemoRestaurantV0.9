using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoRestaurant.Models
{
    public class CheckOutViewModel
    {
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public decimal ToTalPrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime ShippingDate { get; set; }

    }
}