using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Personas
{
    public abstract class WellKnownOrder
    {
        public abstract int Id { get; }
        public abstract int ClientId { get; }
        public abstract int ProductId { get; }
        public abstract int Quantity { get; }
        public abstract decimal UnitPrice { get; }
        public abstract string DeliveryAddress { get; }
        public abstract OrderStatus OrderStatus { get; }
    }
}
