using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homework4.Contracts;
using Homework4.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Homework4.Controllers
{
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrders([FromBody] Order[] orders)
        {
            await _orderService.Save(orders);

            return Ok();
        }

        [HttpGet]
        public async IAsyncEnumerable<Order> FindOrders(
            [FromQuery] long storehouseId,
            [FromQuery] OrderStatus status,
            [FromQuery] DateTime createdFrom,
            [FromQuery] DateTime createdTo)
        {
            var orders = _orderService.Find(storehouseId, status, createdFrom, createdTo);

            await foreach (var order in orders)
            {
                yield return order;
            }
        }
    }
}