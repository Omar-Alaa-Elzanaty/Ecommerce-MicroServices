namespace OrderApi.Application.DTOs
{
    public record OrderDto(
        int Id,
        int ProductId,
        int ClientId,
        int PurchaseQuantity,
        DateTime OrderDate
        );
}
