using FarmTradeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmTradeDataLayer.Repository
{
    public interface ICartRepo
    {
        void AddToCart(Cart cartItem, Guid? userId);
        IEnumerable<Cart> GetCartDetails(Guid? id);
        void UpdateQuantity(int productId,int Quantity,Guid? userId);
    }
}
