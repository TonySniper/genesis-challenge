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
        public void ItShouldBeCreated()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .BuildDTO();

            var orderOnDatabase = _orderService.CreateOrder(order);

            Assert.IsNotNull(orderOnDatabase);
            Assert.AreEqual(OrderStatus.Pending, orderOnDatabase.OrderStatus);
            Assert.AreEqual(order.ProductId, orderOnDatabase.ProductId);
            Assert.AreEqual(order.Quantity, orderOnDatabase.Quantity);
            Assert.AreEqual(order.UnitPrice, orderOnDatabase.UnitPrice);
            Assert.AreEqual(order.CustomerId, orderOnDatabase.CustomerId);
            Assert.AreEqual(order.DeliveryAddress, orderOnDatabase.DeliveryAddress);
        }

        [TestMethod]
        public void GivenAUserHasCompletedOrdersExceedingTheValueOfOneHundredEuroItShouldNotBeRejected()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .BuildDTO();

            AssumeOrdersCreatedWithCompletedStatusExceeds100EuroForClient(order.CustomerId);

            var orderOnDatabase = _orderService.CreateOrder(order);

            Assert.IsNotNull(orderOnDatabase);
            Assert.AreEqual(OrderStatus.Pending, orderOnDatabase.OrderStatus);
            Assert.AreEqual(order.ProductId, orderOnDatabase.ProductId);
            Assert.AreEqual(order.Quantity, orderOnDatabase.Quantity);
            Assert.AreEqual(order.UnitPrice, orderOnDatabase.UnitPrice);
            Assert.AreEqual(order.CustomerId, orderOnDatabase.CustomerId);
            Assert.AreEqual(order.DeliveryAddress, orderOnDatabase.DeliveryAddress);
        }

        [TestMethod]
        public void GivenAClientWithOutstandingOrdersExceeding100EuroItShouldBeRejected()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .IgnoreId()
                .BuildDTO();

            string expectedErrorMessage = $"Order was rejected because customer with id {order.CustomerId} has outstanding orders with a total value in excess of one hundred Euro";

            this.AssumeOrdersCreatedWithUnitPriceAndQuantityForClient(5, order.CustomerId, 25, 2);

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
                .BuildDTO();

            string expectedErrorMessage = $"Order was rejected because it contains more than 10 units of product { order.ProductId }";


            var exception = Assert.ThrowsException<Exception>(() => { _orderService.CreateOrder(order); });
            Assert.AreEqual(expectedErrorMessage, exception.Message);
        }

        [TestMethod]
        public void GivenAnOrderHasNoProductIdItShouldThrowAnException()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .WithProductId(0)   
                .BuildDTO();

            string expectedErrorMessage = $"Product ID is required";

            var exception = Assert.ThrowsException<Exception>(() => { _orderService.CreateOrder(order); });
            Assert.AreEqual(expectedErrorMessage, exception.Message);
        }

        [TestMethod]
        public void GivenAnOrderHasEmptyOrInvalidRequiredFieldsItShouldThrowAnException()
        {
            var newOrder = new StandardOrder();

            var order = new OrderBuilder()
                .With(newOrder)
                .WithProductId(0)
                .WithCustomerId(0)
                .WithQuantity(0)
                .WithUnitPrice(0)
                .BuildDTO();

            order.DeliveryAddress = null;

            Assert.ThrowsException<Exception>(() => { _orderService.CreateOrder(order); });
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

        private void AssumeOrdersCreatedWithCompletedStatusExceeds100EuroForClient(int clientId)
        {
            var newExpensiveOrder = new StandardOrder();

            for (int i = 0; i < 5; i++)
            {
                var order = new OrderBuilder()
                    .With(newExpensiveOrder)
                    .WithCustomerId(clientId)
                    .WithQuantity(5)
                    .WithUnitPrice(25)
                    .WithOrderStatus(OrderStatus.Completed)
                    .IgnoreId()
                    .BuildEntity();

                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }

        private void AssumeOrdersCreatedWithUnitPriceAndQuantityForClient(int numberOfOrders, int clientId, decimal unitPrice, int quantity)
        {
            var newExpensiveOrder = new StandardOrder();

            for (int i = 0; i < numberOfOrders; i++)
            {
                var order = new OrderBuilder()
                    .With(newExpensiveOrder)
                    .WithCustomerId(clientId)
                    .WithQuantity(quantity)
                    .WithUnitPrice(unitPrice)
                    .IgnoreId()
                    .BuildEntity();

                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }
    }
}
