using System.ComponentModel.DataAnnotations;

namespace Project.WebUI.Models
{
    public class MaterialModel
    {
        [Display(Name = "Material ID")]
        public int Id { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Stock")]
        public int Stock { get; set; }
        [Display(Name = "Unit of measure")]
        public string UnitOfMeasure { get; set; }
        [Display(Name = "Weight")]
        public int Weight { get; set; }
        [Display(Name = "Price")]
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
