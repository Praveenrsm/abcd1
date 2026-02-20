using FarmTradeDataLayer;
using FarmTradeDataLayer.Repository;
using FarmTradeEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FarmBusiness.Services
{
    public class OrderService
    {
        IOrderRepo _orderRepo;
        FarmContext _context;
        public OrderService(IOrderRepo orderRepo, FarmContext context)
        {
            _orderRepo = orderRepo;
            _context = context;
        }

        public void CancelOrder(int orderId, Guid userId)
        {
            _orderRepo.UpdateOrderStatus(orderId, userId, OrderStatus.cancelled);
        }


        public OrderDetails GetOrderDetails(int orderId,Guid userId)
        {
           return _orderRepo.GetOrderDetails(orderId,userId);
        }
        public int PlaceOrder(Guid userId, int addressId)
        {
            var order = new Order
            {
                userId = userId,
                addressId = addressId,
                PaymentMethod = "Cash On Delivery"
            };

            // 🔑 Capture returned order WITH Id
            var createdOrder = _orderRepo.AddOrder(order);

            if (createdOrder == null)
                throw new Exception("Order creation failed");

            var cartItems = _context.cart
                .Where(c => c.userId == userId)
                .ToList();

            foreach (var item in cartItems)
            {
                var productPrice = _context.product
                    .Where(p => p.ProductId == item.productId)
                    .Select(p => p.productPrice)
                    .First();

                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    productId = item.productId,
                    Quantity = item.Quantity,
                    UnitPrice = productPrice,                    // ✅ ONE unit price
                    TotalPrice = productPrice * item.Quantity    // ✅ correct total
                };

                _context.orderItem.Add(orderItem);
            }

            _context.cart.RemoveRange(cartItems);
            _context.SaveChanges();

            NotifySuppliers(createdOrder.Id);
            return createdOrder.Id;
        }


        private void NotifySuppliers(int orderId)
        {
            var orderItems = _context.orderItem
                .Where(oi => oi.OrderId == orderId)
                .ToList();

            var suppliers = _context.product
                .Include(p => p.User)
                .Where(p => orderItems.Select(oi => oi.productId).Contains(p.ProductId))
                .ToList();

            foreach (var supplier in suppliers)
            {
                // ✅ SAFETY CHECK
                if (supplier.User == null || string.IsNullOrEmpty(supplier.User.Email))
                    continue; // skip silently

                string subject = $"New Order #{orderId}";
                string body = BuildOrderNotificationEmailBody(supplier, orderId, orderItems);

                SendEmail(supplier.User.Email, subject, body);
            }
        }

        private string BuildOrderNotificationEmailBody(Product supplier, int orderId, List<OrderItem> orderItems)
        {
            var sb = new StringBuilder();
            if (supplier.User == null || string.IsNullOrEmpty(supplier.User.UserName))
            {
                sb.AppendLine("Hello Supplier,");
            }
            else
            {
                sb.AppendLine($"Hello {supplier.User.UserName},");
            }

            sb.AppendLine();
            sb.AppendLine($"You have received a new order (Order ID: {orderId}). Here are the details:");
            sb.AppendLine();

            foreach (var item in orderItems.Where(oi => oi.productId == supplier.ProductId))
            {
                sb.AppendLine($"- Product ID: {item.productId}, Quantity: {item.Quantity}, Price: {item.UnitPrice:C}");
            }

            sb.AppendLine();
            sb.AppendLine("Please prepare for fulfillment.");
            sb.AppendLine("Thank you!");

            return sb.ToString();
        }

        private void SendEmail(string recipientEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.your-email-provider.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("praveenrsm1234@gmail.com", "Praveen0077$$"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("Praveenrsm1234@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(recipientEmail);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {recipientEmail}. Error: {ex.Message}");
                // Optionally log the exception or handle it as needed
            }
        }

    }
}
