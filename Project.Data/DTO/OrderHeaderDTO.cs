using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DTO
{
    public class OrderHeaderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }  
    }
}
