namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDto(
        int OrderId,
        int ProductId,
        int ClientId,
        string Email,
        string Address,
        string TelphoneNumber,
        string ProductName,
        int PurchaseQuantity,
        decimal UnitPrice,
        decimal TotalPrice,
        DateTime OrderedDate
        );
}
