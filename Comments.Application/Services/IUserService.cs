using Comments.Domain.DTOs;
using Comments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Application.Services
{
    public interface IUserService
    {
        Task<User> GetUser(UserDto userDto);
        Task<User> AddUser(UserDto userDto);

    }
}
