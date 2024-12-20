namespace Project.WebUI.Models
{
    public class MaterialModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }

        public MaterialModel(int id, string description, int stock, string unitOfMeasure, int weight, double price)
        {
            Id = id;
            Description = description;
            Stock = stock;
            UnitOfMeasure = unitOfMeasure;
            Weight = weight;
            Price = price;
        }
    }
}
