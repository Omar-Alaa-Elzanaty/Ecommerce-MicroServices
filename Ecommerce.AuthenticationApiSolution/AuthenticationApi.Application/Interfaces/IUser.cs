using AuthenticationApi.Application.DTOs;
using eCommerce.SharedLibrary.Response;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDto appUser);
        Task<Response>Login(LoginDto loginDto);
        Task<GetUserDto> GetUser(int userId);
    }
}
