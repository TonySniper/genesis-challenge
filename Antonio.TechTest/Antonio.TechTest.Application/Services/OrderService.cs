using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.DataAccess.Context;
using System;
using System.Collections.Generic;
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
            _context.Add(order);
            _context.SaveChanges();
        }
    }
}
