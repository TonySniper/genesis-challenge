using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antonio.TechTest.Application.Services;
using Antonio.TechTest.Core.DTO;
using Antonio.TechTest.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Antonio.TechTest.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _orderService.GetOrderByQuery(new OrderQueryRequestDTO
            {
                OrderId = id
            }).FirstOrDefault();

            if (order == null)
                return NotFound($"Order with with {id} was not found");
                        
            return Ok(order);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var orders = _orderService.GetAllOrders();
            
            return Ok(orders);
        }

        [HttpPost("query")]
        public IActionResult Query([FromBody] OrderQueryRequestDTO dto)
        {
            var orders = _orderService.GetOrderByQuery(dto);

            if (orders.Any())
                return Ok(orders);

            return NotFound($"No orders matches the current search criteria");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]CreateOrderRequestDTO value)
        {
            try
            {
                var order = _orderService.CreateOrder(value);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}