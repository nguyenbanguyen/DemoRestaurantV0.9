using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DemoRestaurant.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Tên món ko được để trống")]
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        [Required(ErrorMessage ="Đơn giá ko được để trống")]
        [Range(0,double.MaxValue,ErrorMessage ="Xin vui lòng nhập giá tiền là số dương")]
        [RegularExpression("^[0-9]*$",ErrorMessage = "Xin vui lòng nhập giá tiền là số dương")]
        public int Price { get; set; }
        [Required]
        [Range(0,100,ErrorMessage ="Xin vui lòng nhập số dương ko quá 100%")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Xin vui lòng nhập số dương")]
        public int Discount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Xin vui lòng nhập số dương ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Xin vui lòng nhập số dương")]

        public int MaximunQuantity { get; set; }
        //Cần update ảnh cho category/product/customer
        //public string Image { get; set; }
        // check total sold, need fix later for checking selling report
        public int TotalSold { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

    }
}