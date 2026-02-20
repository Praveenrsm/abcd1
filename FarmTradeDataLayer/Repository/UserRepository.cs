using FarmTradeEntity;
using Microsoft.EntityFrameworkCore;

namespace FarmTradeDataLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        FarmContext _farmcontext;
        public UserRepository(FarmContext context)
        {
            _farmcontext = context;
        }

        public async Task Adduser(User user)
        {
            #region ADD ADMIN,SUPPLIER,USER
            //user.password = Hash(user.password);
            await _farmcontext.users.AddAsync(user);
            await _farmcontext.SaveChangesAsync();
            #endregion
            //create a db for user have to add only username,password

        }
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<User> Login(string Email)
        {
            #region LOGIN 
            return await _farmcontext.users.FirstOrDefaultAsync(u => u.Email == Email);
            //List<User> list = new List<User>();
            //list = _farmcontext.users.ToList();
            //foreach (var user in list)
            //{
            //    if (user.Email == users.Email)
            //    {
            //        if (BCrypt.Net.BCrypt.Verify(users.password, user.password))
            //        {
            //            //string role = user.role;
            //            return user;
            //        }
            //    }
            //}
            //return null;
            #endregion

        }
        public async Task Updateuser(User user)
        {
            #region EDIT PROFILE AFTER LOGIN 
            //_farmcontext.users.Update(user);
            _farmcontext.Entry(user).State = EntityState.Modified;
            await _farmcontext.SaveChangesAsync();
            #endregion
        }

        public async Task<User> GetUserById(Guid userId)
        {
            #region GET UNIQUE PROFILE 
            var result = await _farmcontext.users.ToListAsync();
            var user =  result.Where(obj => obj.UserId == userId).FirstOrDefault();
            return user;
            #endregion

        }

        public async Task<IEnumerable<User>> Getusers(int pageNumber,int pageSize)
        {
            #region GET All USER
            return await _farmcontext.users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            #endregion
        }
        public async Task DeleteUser(Guid userId)
        {
            #region DELETE USER
            var user = await _farmcontext.users.FindAsync(userId);
            //var userId = user.UserId;
            //return userId;
            if (user == null)
            {
                return;
            }
            _farmcontext.users.Remove(user);
            await _farmcontext.SaveChangesAsync();
            #endregion
        }

    }
}
