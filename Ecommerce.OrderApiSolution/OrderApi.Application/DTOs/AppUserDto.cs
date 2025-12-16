using Microsoft.Identity.Client;

namespace OrderApi.Application.DTOs
{
    public record AppUserDto(
        int Id,
        string TelphoneNumber,
        string Email,
        string Password,
        string Address,
        string Role
        );
}
