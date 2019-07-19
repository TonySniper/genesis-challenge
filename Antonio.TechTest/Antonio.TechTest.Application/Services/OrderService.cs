using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antonio.TechTest.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly TechTestContext _context;

        public OrderService(TechTestContext context)
        {
            _context = context;
        }

        public void CreateOrder(Order order)
        {
            if (!IsClientAbleToPlaceOrders(order.ClientId))
                throw new Exception($"Order was rejected because Client with id {order.ClientId} has outstanding orders with a total value in excess of onde hundred Euro");

            if (order.Quantity > 10)
                throw new Exception($"Order was rejected because it contains more than 10 units of product { order.ProductId }");

            _context.Add(order);
            _context.SaveChanges();
        }

        private bool IsClientAbleToPlaceOrders(int clientId)
        {
            var totalValue = _context.Orders.Where(x => x.ClientId == clientId).Sum(x => x.Quantity * x.UnitPrice);
            return totalValue <= 1000;
        }
    }
}
