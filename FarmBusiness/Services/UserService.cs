using FarmTradeDataLayer.Repository;
using FarmTradeEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FarmBusiness.Services
{
    public class UserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // CRUD Service Operations for User:
        public async Task UpdateUser(User user)
        {
           await _userRepository.Updateuser(user);
        }
        public async Task AddUser(User user)
        {
            await _userRepository.Adduser(user);
        }
        public async Task<User> Login(string email,string password)
        {
            var result = await _userRepository.Login(email);
            if (result == null) // || !BCrypt.Net.BCrypt.Verify(password, result.password)
                return null;
            return result;
        }
        public async Task<User> GetUserById(Guid userId)
        {
            return await _userRepository.GetUserById(userId);
        }
        public async Task<IEnumerable<User>> GetUsers(int pageNumber, int pageSize)
        {
            return await _userRepository.Getusers(pageNumber, pageSize);
        }
        public async Task DeleteUser(Guid userId)
        {
           await _userRepository.DeleteUser(userId);
        }

        public async Task<User> Register(string email, string password, string role)
        {
            var existingUser = await _userRepository.Login(email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Email = email,
                password = BCrypt.Net.BCrypt.HashPassword(password),
                role = role
            };

            await _userRepository.Adduser(user);
            return user;
        }
    }
}
