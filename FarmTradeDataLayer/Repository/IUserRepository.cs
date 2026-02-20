using FarmTradeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FarmTradeDataLayer.Repository
{
    public interface IUserRepository
    {
        Task Updateuser(User user);
        Task Adduser(User user);
        Task<User> Login(string Email);
        Task<User> GetUserById(Guid userId);
        Task<IEnumerable<User>> Getusers(int pageNumber, int pageSize);
        Task DeleteUser(Guid userId);
    }
}
