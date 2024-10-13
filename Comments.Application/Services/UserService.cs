using Comments.Application.Repositories;
using Comments.Domain.DTOs;
using Comments.Domain.Entities;

namespace Comments.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<User> AddUser(UserDto userDto)
        {

            User user = new User() 
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
            };

            return await userRepository.CreateAsync(user);
        }

        public async Task<User> GetUser(UserDto userDto)
        {
            User user = new User()
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
            };
            return await userRepository.GetAsync(user);
        }
    }
}
