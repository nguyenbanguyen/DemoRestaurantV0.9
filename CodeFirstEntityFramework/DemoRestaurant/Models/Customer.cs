using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoRestaurant.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }
        [Required]
        public String CustomerName { get; set; }
        [DataType(DataType.PhoneNumber,ErrorMessage = "Vui lòng nhập chính xác số điện thoại")]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "Số điện thoại phải từ 9 đến 11 số")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Vui lòng nhập chính xác số điện thoại")]

        public string CustomerPhone { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập vị trí")]
        public String ShippingAddress { get; set; }
        public String CustomerType { get; set; }
        public Double? Account { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        
    }
}