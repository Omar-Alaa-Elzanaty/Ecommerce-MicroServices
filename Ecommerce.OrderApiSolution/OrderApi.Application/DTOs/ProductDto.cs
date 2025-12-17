namespace OrderApi.Application.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        int Quantity,
        decimal Price
        );
}
