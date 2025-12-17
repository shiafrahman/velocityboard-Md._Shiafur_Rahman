using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.UserDtos;
using VelocityBoard.Application.Interfaces;
using VelocityBoard.Core.Models;
using VelocityBoard.Infrastructure.Data;

namespace VelocityBoard.Application.Services
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> RegisterUserAsync(CreateUserDto createUserDto)
        {
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == createUserDto.UserName || u.Email == createUserDto.Email);

            if (existingUser != null)
            {
                return null; 
            }

            var user = _mapper.Map<User>(createUserDto);
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
