using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Personas.OrderQuery
{
    public abstract class WellKnownOrderQueryRequest
    {
        public abstract int OrderId { get; }
        public abstract int CustomerId { get; }
        public abstract int ProductId { get; }
        public abstract string DeliveryAddress { get; }
    }
}
