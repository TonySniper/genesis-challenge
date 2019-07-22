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
        private bool _withClientId;
        private bool _withProductId;
        private bool _withUnitPrice;
        private bool _withProductQuantity;
        private bool _withOrderStatus;

        private decimal _unitPrice = 0;
        private int _productQuantity = 0;
        private int _clientId = 0;
        private int _productId = 0;
        private OrderStatus _orderStatus;

        public OrderBuilder With(WellKnownOrder wellKnownOrder)
        {
            _wellKnownOrder = wellKnownOrder;
            return this;
        }

        public OrderBuilder WithClientId(int id)
        {
            _clientId = id;
            _withClientId = true;
            return this;
        }

        public OrderBuilder WithProductId(int id)
        {
            _productId = id;
            _withProductId = true;
            return this;
        }

        public OrderBuilder IgnoreId()
        {
            _ignoreId = true;
            return this;
        }

        public OrderBuilder WithUnitPrice(decimal price)
        {
            _unitPrice = price;
            _withUnitPrice = true;
            return this;
        }

        public OrderBuilder WithQuantity(int quantity)
        {
            _productQuantity = quantity;
            _withProductQuantity = true;
            return this;
        }

        public OrderBuilder WithOrderStatus(OrderStatus status)
        {
            _orderStatus = status;
            _withOrderStatus = true;
            return this;
        }

        public Order Build()
        {
            var order = new Order
            {
                CustomerId = _wellKnownOrder.ClientId,
                DeliveryAddress = _wellKnownOrder.DeliveryAddress,
                OrderStatus = _wellKnownOrder.OrderStatus,
                ProductId = _wellKnownOrder.ProductId,
                Quantity = _wellKnownOrder.Quantity,
                UnitPrice = _wellKnownOrder.UnitPrice
            };

            if (_ignoreId)
                order.Id = _wellKnownOrder.Id;

            if (_withClientId)
                order.CustomerId = _clientId;

            if (_withProductQuantity)
                order.Quantity = _productQuantity;

            if (_withUnitPrice)
                order.UnitPrice = _unitPrice;

            if (_withOrderStatus)
                order.OrderStatus = _orderStatus;

            if (_withProductId)
                order.ProductId = _productId;

            return order;
        }
    }
}
