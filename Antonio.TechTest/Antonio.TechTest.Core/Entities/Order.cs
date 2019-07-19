using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string DeliveryAddress { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 1,
        Completed = 2
    }
}
