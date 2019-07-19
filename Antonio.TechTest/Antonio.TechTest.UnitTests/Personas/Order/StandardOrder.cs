using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Personas
{
    public class StandardOrder : WellKnownOrder
    {
        public override int Id => 0;
        public override int ClientId => 1;
        public override int ProductId => 1;
        public override int Quantity => 2;
        public override decimal UnitPrice => 10;
        public override string DeliveryAddress => "Dummy Address";
        public override OrderStatus OrderStatus => OrderStatus.Pending;
    }
}
