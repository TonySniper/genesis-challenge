using Antonio.TechTest.Application.Mapping;
using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using Antonio.TechTest.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Antonio.TechTest.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly TechTestContext _context;

        public OrderService(TechTestContext context)
        {
            _context = context;
        }

        public Order CreateOrder(CreateOrderRequestDTO orderDto)
        {
            var order = OrderMap.Map(orderDto);

            this.ValidateOrder(order);

            order.OrderStatus = OrderStatus.Pending;

            _context.Add(order);
            _context.SaveChanges();

            return order;
        }
        
        public IEnumerable<OrderQueryResponseDTO> GetOrderByQuery(OrderQueryRequestDTO dto)
        {
            var queryableOrders = _context.Orders.AsQueryable<Order>();

            if (dto == null)
                throw new Exception("OrderQuery DTO cannot be null");

            if (dto.CustomerId != 0)
                queryableOrders = queryableOrders.Where(x => x.CustomerId == dto.CustomerId);

            if (!string.IsNullOrWhiteSpace(dto.DeliveryAddress))
                queryableOrders = queryableOrders.Where(x => x.DeliveryAddress.Contains(dto.DeliveryAddress));

            if (dto.OrderId != 0)
                queryableOrders = queryableOrders.Where(x => x.Id == dto.OrderId);

            if (dto.ProductId != 0)
                queryableOrders = queryableOrders.Where(x => x.ProductId == dto.ProductId);

            var orderList = queryableOrders.ToList();
            
            var result = orderList.Select(x => new OrderQueryResponseDTO
            {
                ProductId = x.ProductId,
                CustomerId = x.CustomerId,
                DeliveryAddress = x.DeliveryAddress,
                OrderId = x.Id,
                OrderStatus = x.OrderStatus,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            });

            return result;
        }

        public IEnumerable<OrderQueryResponseDTO> GetAllOrders()
        {
            return this.GetOrderByQuery(new OrderQueryRequestDTO());
        }

        private bool IsClientAbleToPlaceOrders(int clientId)
        {
            var totalValue = _context.Orders.Where(x => x.CustomerId == clientId && x.OrderStatus == OrderStatus.Pending).Sum(x => x.Quantity * x.UnitPrice);
            return totalValue <= 100;
        }

        private void ValidateOrder(Order order)
        {
            if (order.ProductId == 0)
                throw new Exception("Product ID is required");

            if (order.Quantity <= 0)
                throw new Exception("Quantity cannot be lower or equal to zero");

            if (order.UnitPrice <= 0)
                throw new Exception("Unit price cannot be lower or equal to zero");

            if (string.IsNullOrWhiteSpace(order.DeliveryAddress))
                throw new Exception("Delivery Address cannot be empty");

            if (!IsClientAbleToPlaceOrders(order.CustomerId))
                throw new Exception($"Order was rejected because customer with id {order.CustomerId} has outstanding orders with a total value in excess of one hundred Euro");

            if (order.Quantity > 10)
                throw new Exception($"Order was rejected because it contains more than 10 units of product {order.ProductId}");
        }
    }
}
