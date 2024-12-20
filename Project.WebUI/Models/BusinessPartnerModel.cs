using System.ComponentModel.DataAnnotations;

namespace Project.WebUI.Models
{
    public class BusinessPartnerModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Incoterms { get; set; } = string.Empty;
        public string PaymentTerms { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public BusinessPartnerModel()
        {
        }

        public BusinessPartnerModel(int id, string name, string address, string incoterms, string paymentTerms, string role)
        {
            Id = id;
            Name = name;
            Address = address;
            Incoterms = incoterms;
            PaymentTerms = paymentTerms;
            Role = role;
        }

    }
}
