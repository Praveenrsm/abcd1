using FarmTradeEntity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FarmTradeDataLayer.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly FarmContext _context;

        public OrderRepo(FarmContext context)
        {
            _context = context;
        }

        // ADD ORDER
        public Order AddOrder(Order order)
        {
            var result = _context.orders
                .FromSqlRaw(
                    "EXEC sp_AddOrder @UserId={0}, @AddressId={1}, @PaymentMethod={2}",
                    order.userId,
                    order.addressId,
                    order.PaymentMethod
                )
                .AsEnumerable()
                .FirstOrDefault();

            return result;
        }

        // GET ORDER BY ID
        public Order GetOrderById(int orderId)
        {
            return _context.orders
                .FromSqlRaw("EXEC sp_GetOrderById @OrderId={0}", orderId)
                .AsNoTracking()
                .FirstOrDefault();
        }

        // GET ALL ORDERS
        public IEnumerable<Order> GetOrders()
        {
            return _context.orders
                .FromSqlRaw("EXEC sp_GetAllOrders")
                .AsNoTracking()
                .ToList();
        }

        // UPDATE ORDER STATUS
        public void UpdateOrderStatus(int orderId, OrderStatus status)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_UpdateOrderStatus @OrderId={0}, @Status={1}",
                orderId,
                (int)status
            );
        }

        public OrderDetails GetOrderDetails(int orderId,Guid userId)
        {
            var order = _context.orders.Where(o => o.Id == orderId && o.userId == userId)
                .Include(o => o.address)
                .FirstOrDefault(o => o.Id == orderId);
            if(order == null)
            {
                return null;
            }
            var items = _context.orderItem
                .Include(oi => oi.product)
                .Where(oi => oi.OrderId == orderId)
                .Select(oi => new OrderItem
                {
                    ProductName = oi.product.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.Quantity * oi.UnitPrice
                })
                .ToList();

            return new OrderDetails
            {
                OrderId = order.Id,
                OrderDate = order.orderDate,
                Status = order.status.ToString(),
                Address = order.address,
                Items = items,
                GrandTotal = items.Sum(i => i.TotalPrice)
            };
        }


        public void UpdateOrderStatus(int orderId, Guid userId, OrderStatus status)
        {
            var order = _context.orders
                .FirstOrDefault(o => o.Id == orderId && o.userId == userId);

            if (order == null)
                throw new Exception("Order not found");

            order.status = status;
            _context.SaveChanges();
        }

    }
}
