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
    public class WhenCreatingAnOrder
    {
        private IOrderService _orderService;
        private TechTestContext _context;

        public WhenCreatingAnOrder()
        {
            var randomDatabaseName = Guid.NewGuid().ToString();
            var dbOptions = new DbContextOptionsBuilder<TechTestContext>().UseInMemoryDatabase(randomDatabaseName).Options;
            _context = new TechTestContext(dbOptions);
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
    }
}
