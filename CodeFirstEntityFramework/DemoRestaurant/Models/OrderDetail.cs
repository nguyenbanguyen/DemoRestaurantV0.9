using System.ComponentModel.DataAnnotations.Schema;

namespace DemoRestaurant.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int Total { get; set; }       
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

    }
        
}