using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Core.DTO
{
    public class OrderQueryResponseDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string DeliveryAddress { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string OrderStatusDescription
        {
            get
            {
                return OrderStatus == OrderStatus.Completed ? "Completed" : "Pending";
            }
        }


        public decimal OrderTotal
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }
    }
}
