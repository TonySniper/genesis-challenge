using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Core.DTO
{
    public class CreateOrderRequestDTO
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
