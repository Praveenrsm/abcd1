using FarmTradeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmTradeDataLayer.Repository
{
    public interface IOrderRepo
    {
        void UpdateOrderStatus(int OrderId,OrderStatus orderStatus);
        Order AddOrder(Order order);
        Order GetOrderById(int orderId);
        IEnumerable<Order> GetOrders();
        OrderDetails GetOrderDetails(int orderId,Guid userId);
        void UpdateOrderStatus(int orderId, Guid userId, OrderStatus status);
    }
}
