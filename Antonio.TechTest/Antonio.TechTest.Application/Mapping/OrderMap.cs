using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Application.Mapping
{
    public static class OrderMap
    {
        public static Order Map(CreateOrderRequestDTO dto)
        {
            return new Order
            {
                CustomerId = dto.CustomerId,
                DeliveryAddress = dto.DeliveryAddress,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };
        }
    }
}
