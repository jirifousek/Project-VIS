using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DTO
{
    public class BusinessPartnerDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Incoterms { get; set; }
        public String PaymentTerms { get; set; }
        public String Role { get; set; }
        public String Address { get; set; }
    }
}
