using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.UnitTests.Personas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Builders
{
    public class OrderBuilder
    {
        private WellKnownOrder _wellKnownOrder;
        private bool _ignoreId;

        public OrderBuilder With(WellKnownOrder wellKnownOrder)
        {
            _wellKnownOrder = wellKnownOrder;
            return this;
        }

        public OrderBuilder IgnoreId()
        {
            _ignoreId = true;
            return this;
        }

        public Order Build()
        {
            var order = new Order
            {
                ClientId = _wellKnownOrder.ClientId,
                DeliveryAddress = _wellKnownOrder.DeliveryAddress,
                OrderStatus = _wellKnownOrder.OrderStatus,
                ProductId = _wellKnownOrder.ProductId,
                Quantity = _wellKnownOrder.Quantity,
                UnitPrice = _wellKnownOrder.UnitPrice
            };

            if (_ignoreId)
                order.Id = _wellKnownOrder.Id;

            return order;
        }
    }
}
