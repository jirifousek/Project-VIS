using System.ComponentModel.DataAnnotations;

namespace Project.WebUI.Models
{
    public class OrderItemModel
    {
        [Display(Name = "Item no")]
        public int Id { get; set; }
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        public MaterialModel Material { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}
