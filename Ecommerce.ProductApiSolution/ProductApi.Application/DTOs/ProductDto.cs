namespace ProductApi.Application.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        decimal Price,
        int Quantity
    );
}
