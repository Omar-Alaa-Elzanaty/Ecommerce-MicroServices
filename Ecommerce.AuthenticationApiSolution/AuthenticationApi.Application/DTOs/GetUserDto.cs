namespace AuthenticationApi.Application.DTOs
{
    public record GetUserDto(
    int Id,
    string Name,
    string TelphoneNumber,
    string Address,
    string Email,
    string Password,
    string Role);
}
