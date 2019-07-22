using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Core.DTO
{
    public class OrderQueryRequestDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
