using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.UnitTests.Personas.OrderQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Builders
{
    public class OrderQueryRequestBuilder
    {
        private WellKnownOrderQueryRequest _wellKnownOrderQuery;

        private string _deliveryAddress;
        private int _productId;
        private int _customerId;

        private bool _withDeliveryAddress;
        private bool _withProductId;
        private bool _withCustomerId;

        public OrderQueryRequestBuilder WithDeliveryAddress(string deliveryAddress)
        {
            _deliveryAddress = deliveryAddress;
            _withDeliveryAddress = true;

            return this;
        }

        public OrderQueryRequestBuilder WithProductId(int productId)
        {
            _productId = productId;
            _withProductId = true;

            return this;
        }

        public OrderQueryRequestBuilder WithCustomerId(int customerId)
        {
            _customerId = customerId;
            _withCustomerId = true;

            return this;
        }

        public OrderQueryRequestBuilder With(WellKnownOrderQueryRequest wellKnownOrderQuery)
        {
            _wellKnownOrderQuery = wellKnownOrderQuery;
            return this;
        }

        public OrderQueryRequestDTO Build()
        {
            var query = new OrderQueryRequestDTO
            {
                CustomerId = _wellKnownOrderQuery.CustomerId,
                DeliveryAddress = _wellKnownOrderQuery.DeliveryAddress,
                OrderId = _wellKnownOrderQuery.OrderId,
                ProductId = _wellKnownOrderQuery.ProductId
            };

            if (_withCustomerId)
                query.CustomerId = _customerId;

            if (_withProductId)
                query.ProductId = _productId;

            if (_withDeliveryAddress)
                query.DeliveryAddress = _deliveryAddress;
                       
            return query;
        }
    }
}
