using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelocityBoard.Application.DTOs;

namespace VelocityBoard.Application.Interfaces
{
    public interface IAuthService
    {
        string? Login(LoginDto loginDto);
    }
}
