namespace Project.WebUI.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public MaterialModel Material { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
