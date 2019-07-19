using Antonio.TechTest.Application;
using Antonio.TechTest.Application.Services;
using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.DataAccess;
using Antonio.TechTest.DataAccess.Context;
using Antonio.TechTest.UnitTests.Builders;
using Antonio.TechTest.UnitTests.Personas;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonio.TechTest.UnitTests.Services
{
    [TestClass]
    public class WhenCreatingAnOrder : UnitTestBase
    {
        private IOrderService _orderService;

        public WhenCreatingAnOrder()
        {
            _orderService = new OrderService(_context);
        }

        [TestMethod]
        public void ItShouldBeCreatedOnTheDatabase()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .Build();

            _orderService.CreateOrder(order);

            var orderOnDatabase = _context.Orders.AsNoTracking().FirstOrDefault(x => x.Id == order.Id);

            Assert.IsNotNull(orderOnDatabase);
            Assert.AreEqual(order.OrderStatus, orderOnDatabase.OrderStatus);
            Assert.AreEqual(order.ProductId, orderOnDatabase.ProductId);
            Assert.AreEqual(order.Quantity, orderOnDatabase.Quantity);
            Assert.AreEqual(order.UnitPrice, orderOnDatabase.UnitPrice);
            Assert.AreEqual(order.ClientId, orderOnDatabase.ClientId);
            Assert.AreEqual(order.DeliveryAddress, orderOnDatabase.DeliveryAddress);
        }

        [TestMethod]
        public void GivenAClientWithOutstandingOrdersExceeding1000EuroItShouldBeRejected()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .IgnoreId()
                .Build();

            string expectedErrorMessage = $"Could not create the order because Client with id {order.ClientId} has outstanding orders with a total value in excess of onde hundred Euro";

            this.AssumeExpensiveOrdersCreatedOnDatabaseForClient(order.ClientId, 200, 2);

            var exception = Assert.ThrowsException<Exception>(() => { _orderService.CreateOrder(order); });
            Assert.AreEqual(expectedErrorMessage, exception.Message);
        }

        [TestMethod]
        public void GivenAnOrderContainMoreThan10OfAProductItShouldBeRejected()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .WithQuantity(11)
                .Build();

            string expectedErrorMessage = $"Order was rejected because it contains more than 10 units of product { order.ProductId }";


            var exception = Assert.ThrowsException<Exception>(() => { _orderService.CreateOrder(order); });
            Assert.AreEqual(expectedErrorMessage, exception.Message);
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

        private void AssumeExpensiveOrdersCreatedOnDatabaseForClient(int clientId, decimal unitPrice, int quantity)
        {
            var newExpensiveOrder = new StandardOrder();

            for (int i = 0; i < 5; i++)
            {
                var order = new OrderBuilder()
                    .With(newExpensiveOrder)
                    .WithClientId(clientId)
                    .WithQuantity(quantity)
                    .WithUnitPrice(unitPrice)
                    .IgnoreId()
                    .Build();

                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }
    }
}
