using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.UnitTests.Personas.OrderQuery
{
    public class StandardQueryRequest : WellKnownOrderQueryRequest
    {
        public override int OrderId => 0;

        public override int CustomerId => 1;

        public override int ProductId => 1;

        public override string DeliveryAddress => "Dummy Address";
    }
}
