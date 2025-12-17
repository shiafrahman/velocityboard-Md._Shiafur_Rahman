using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs;
using VelocityBoard.Application.DTOs.UserDtos;
using VelocityBoard.Application.Interfaces;
using VelocityBoard.Core.Models;
using VelocityBoard.Infrastructure.Data;

namespace VelocityBoard.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string? Login(LoginDto loginDto)
        {
            
            var user = _context.Users.FirstOrDefault(u => u.UserName == loginDto.UserName);

            
            if (user == null)
            {
                return null; 
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash) && user.UserName==loginDto.UserName)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null; 
            }


            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
