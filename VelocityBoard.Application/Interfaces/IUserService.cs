using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs.UserDtos;

namespace VelocityBoard.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> RegisterUserAsync(CreateUserDto createUserDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
