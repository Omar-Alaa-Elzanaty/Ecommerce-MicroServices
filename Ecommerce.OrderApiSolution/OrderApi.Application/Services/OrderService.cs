using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        public async Task<ProductDto> GetProduct(int productId)
        {
            var getProduct = await httpClient.GetAsync($"/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDto>();
            return product!;
        }

        public async Task<AppUserDto>GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"/api/authentication/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDto>();
            return user!;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var order= await orderInterface.FindByIdAsync(orderId);

            if (order is null || order!.Id <= 0)
                return null!;

            var retryPipline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDto = await retryPipline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            var appUserDto = await retryPipline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDto(
                order.Id,
                productDto.Id,
                appUserDto.Id,
                appUserDto.Email,
                appUserDto.Address,
                appUserDto.TelphoneNumber,
                productDto.Name,
                order.PurchaseQuantity,
                productDto.Price,
                order.PurchaseQuantity * productDto.Price,
                order.OrderDate);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetORdersAsync(o => o.ClientId == clientId);

            if (!orders.Any()) return null!;

            var (_, _orders) = OrderConversion.FromEntity(null, orders);

            return _orders!;
        }
    }
}
