using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();

            if (!orders.Any())
                return NotFound("No orders found.");

            var (_, list) = OrderConversion.FromEntity(null, orders);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order is null)
                return NotFound($"Order with ID {id} not found.");

            var (orderDto, _) = OrderConversion.FromEntity(order, null);
            return Ok(orderDto);
        }

        [HttpGet("details/{orderId}")]
        public async Task<ActionResult<OrderDetailsDto>>GetOrderDetails(int orderId)
        {
            var orderDetails = await orderService.GetOrderDetails(orderId);

            if (orderDetails is null)
                return NotFound($"Order details for Order ID {orderId} not found.");

            return Ok(orderDetails);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid order data.");

            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid order data.");

            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await orderInterface.UpdateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<OrderDto>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderService.GetOrdersByClientId(clientId);

            if (orders is null)
                return NotFound($"Order for Client ID {clientId} not found.");

            return Ok(orders);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);

            if (order is null)
                return NotFound($"Order with ID {id} not found.");

            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
