namespace Project.WebUI.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public BusinessPartnerModel BueinessPaetner { get; set; } = new BusinessPartnerModel();
        public List<OrderItemModel> Items { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

        public OrderModel() { }

        public OrderModel(int id, BusinessPartnerModel businessPartner, DateTime expectedDeliveryDate, double totalPrice, string status)
        {
            Id = id;
            BueinessPaetner = businessPartner;
            ExpectedDeliveryDate = expectedDeliveryDate;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}
