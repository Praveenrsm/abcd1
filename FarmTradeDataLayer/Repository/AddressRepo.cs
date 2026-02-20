using FarmTradeEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmTradeDataLayer.Repository
{
    public class AddressRepo : IAddressRepo
    {
        private readonly FarmContext _farmcontext;

        public AddressRepo(FarmContext context)
        {
            _farmcontext = context;
        }
        public async Task AddAddress(Address newAddress, Guid userId)
        {
            newAddress.userId = userId;
            await _farmcontext.address.AddAsync(newAddress);
            await _farmcontext.SaveChangesAsync();
        }
        public IEnumerable<Address> GetAddress(Guid userId)
        {
            return _farmcontext.address
                .Where(a => a.userId == userId)  
                .Include(a => a.user)
                .AsNoTracking()
                .ToList();
        }
        public void UpdateAddress(Address address)
        {
            #region EDIT Address
            _farmcontext.Entry(address).State = EntityState.Modified;
            _farmcontext.SaveChanges();
            #endregion
        }
        public void DeleteAddress(int addressId) 
        {
            #region DELETE Address
            var address = _farmcontext.address.Find(addressId);
            _farmcontext.address.Remove(address);
            _farmcontext.SaveChanges();
            #endregion
        }
        public Address GetAddressById(int id)
        {
            var result=_farmcontext.address.Include(obj => obj.user).ToList();
            var address = result.Where(obj => obj.id == id).FirstOrDefault();
            return address;
        }
    }
}
