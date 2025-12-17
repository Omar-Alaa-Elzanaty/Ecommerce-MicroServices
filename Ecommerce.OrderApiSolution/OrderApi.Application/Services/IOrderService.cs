using OrderApi.Application.DTOs;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByClientId(int clientId);
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
        Task<AppUserDto> GetUser(int userId);
        Task<ProductDto> GetProduct(int productId);
    }
}
