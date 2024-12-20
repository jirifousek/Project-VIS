using System.ComponentModel.DataAnnotations;

namespace Project.WebUI.Models
{
    public class OrderModel
    {
        [Display(Name = "Order ID")]
        public int Id { get; set; }
        public BusinessPartnerModel BusinessPartner { get; set; } = new BusinessPartnerModel();
        public List<OrderItemModel> Items { get; set; }
        [Display(Name = "Expected delivery date")]
        public DateTime ExpectedDeliveryDate { get; set; }
        [Display(Name = "Price")]
        public double TotalPrice { get; set; }
        [Display(Name = "Stataus")]
        public string Status { get; set; }

        public OrderModel() { }

        public OrderModel(int id, BusinessPartnerModel businessPartner, DateTime expectedDeliveryDate, double totalPrice, string status)
        {
            Id = id;
            BusinessPartner = businessPartner;
            ExpectedDeliveryDate = expectedDeliveryDate;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}
