using FarmTradeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmTradeDataLayer.Repository
{
    public interface IAddressRepo
    {
        Task AddAddress(Address address,Guid userId);
        IEnumerable<Address> GetAddress(Guid userId);
        void UpdateAddress(Address address);
        void DeleteAddress(int addressId);
        Address GetAddressById(int addressId);
    }
}
