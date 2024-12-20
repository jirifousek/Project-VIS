﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
