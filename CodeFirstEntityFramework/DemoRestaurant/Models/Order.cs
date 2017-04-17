using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoRestaurant.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public String Username { get; set; }
        public int Discount { get; set; }
        [Required]
        [Range(0,10000000000,ErrorMessage ="Giá tiền ko đúng, vui lòng kiểm tra lại")]
        public int TotalPrice { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime ShippingDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy H:mm}", ApplyFormatInEditMode = true)]

        public DateTime CreatedAt { get; set; }
        public String PaymentInfo { get; set; }
        
        public virtual ICollection<OrderDetail> OrderDetail{ get; set; }
        public virtual Customer Customer { get; set; }
      // tình trạng order public int OrderStatus Status { get; set; }
        



    }
}