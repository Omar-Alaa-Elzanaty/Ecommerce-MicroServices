using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using eCommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser?> GetUserByEmail(string email) =>
             await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<GetUserDto?> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            return user is not null ? new GetUserDto(
                user.Id,
                user.Name,
                user.TelephoneNumber,
                user.Address,
                user.Email,
                user.Password,
                user.Role) : null;
        }

        public async Task<Response> Login(LoginDto loginDto)
        {
            var getUser = await GetUserByEmail(loginDto.Email);

            if (getUser is null)
                return new Response(false, "User with the provided email does not exist.");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, getUser.Password);

            string token = GenerateToken(getUser);

            return verifyPassword
                ? new Response(true, token)
                : new Response(false, "Invalid password.");
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config["Authentication:Key"]!.ToString());
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name,user.Name),
                new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                new (ClaimTypes.Email,user.Email)
            };

            if (!string.IsNullOrEmpty(user.Role))
                claims.Add(new Claim(ClaimTypes.Role, user.Role!));


            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDto appUser)
        {
            var getUser = await GetUserByEmail(appUser.Email);

            if (getUser != null)
                return new Response
                {
                    Flag = false,
                    Message = "User with the provided email already exists."
                };

            var result = await context.Users.AddAsync(new AppUser
            {
                Name = appUser.Name,
                TelephoneNumber = appUser.TelphoneNumber,
                Address = appUser.Address,
                Email = appUser.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUser.Password),
                Role = appUser.Role
            });

            await context.SaveChangesAsync();

            return result.Entity.Id > 0
                ? new Response(true, "User registered successfully.")
                : new Response(false, "User registration failed.");
        }
    }
}
