using Antonio.TechTest.Application.Services;
using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.UnitTests.Builders;
using Antonio.TechTest.UnitTests.Personas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antonio.TechTest.UnitTests.Services
{
    [TestClass]
    public class WhenQueryingForAnOrder : UnitTestBase
    {
        private IOrderService _orderService;

        public WhenQueryingForAnOrder()
        {
            _orderService = new OrderService(_context);
        }

        [TestMethod]
        public void ItShouldReturnAnOrder()
        {
            var existingOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(existingOrder)
                .Build();

            AssumeOrderExists(order);

            var orderOnDatabase = _orderService.GetOrderByQuery(new OrderQueryRequestDTO
            {
                OrderId = order.Id
            }).FirstOrDefault(x => x.OrderId == order.Id);

            Assert.IsNotNull(orderOnDatabase);
            Assert.AreEqual(order.OrderStatus, orderOnDatabase.OrderStatus);
            Assert.AreEqual(order.ProductId, orderOnDatabase.ProductId);
            Assert.AreEqual(order.Quantity, orderOnDatabase.Quantity);
            Assert.AreEqual(order.UnitPrice, orderOnDatabase.UnitPrice);
            Assert.AreEqual(order.CustomerId, orderOnDatabase.CustomerId);
            Assert.AreEqual(order.DeliveryAddress, orderOnDatabase.DeliveryAddress);
        }

        [TestMethod]
        public void GivenQueryForClientIdAndProductIdItShouldReturnASingleOrder()
        {
            int clientId = 10;
            int productId = 20;

            var existingOrder = new StandardOrder();

            var order = new OrderBuilder()
            .With(existingOrder)
            .WithClientId(clientId)
            .WithProductId(productId)
            .Build();

            AssumeOrderExists(order);

            var orderOnDatabase = _orderService.GetOrderByQuery(new OrderQueryRequestDTO
            {
                OrderId = order.Id,
                CustomerId = clientId,
                ProductId = productId
            }).FirstOrDefault(x => x.OrderId == order.Id);

            Assert.IsNotNull(orderOnDatabase);
            Assert.AreEqual(order.OrderStatus, orderOnDatabase.OrderStatus);
            Assert.AreEqual(order.ProductId, orderOnDatabase.ProductId);
            Assert.AreEqual(order.Quantity, orderOnDatabase.Quantity);
            Assert.AreEqual(order.UnitPrice, orderOnDatabase.UnitPrice);
            Assert.AreEqual(order.CustomerId, orderOnDatabase.CustomerId);
            Assert.AreEqual(order.DeliveryAddress, orderOnDatabase.DeliveryAddress);
        }



        private void AssumeOrderExists(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }

        [TestCleanup]
        public void CleanUp()
        {
            foreach (var item in _context.Orders)
            {
                _context.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}
