using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DTO
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public int Stock { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }
    }
}
