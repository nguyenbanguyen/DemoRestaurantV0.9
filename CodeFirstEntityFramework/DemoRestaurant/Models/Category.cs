using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoRestaurant.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Tên Menu ko được để trống")]
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual ICollection<Product> Product{ get; set; }
    }
}