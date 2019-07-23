using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antonio.TechTest.Application.Services
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderRequestDTO orderDto);
        IEnumerable<OrderQueryResponseDTO> GetAllOrders();
        IEnumerable<OrderQueryResponseDTO> GetOrderByQuery(OrderQueryRequestDTO dto);
    }
}
