using FarmTradeEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FarmTradeDataLayer.Repository
{
    public class CartRepo:ICartRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FarmContext _farmcontext;

        public CartRepo(FarmContext context, IHttpContextAccessor httpContextAccessor)
        {
            _farmcontext = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddToCart(Cart cartItem,Guid? userId)
        {
            //Guid? userId = GetLoggedInUserId();
            var product = _farmcontext.product.FirstOrDefault(p => p.ProductId == cartItem.productId);
            if (userId.HasValue)
            {
                // Handle cart for logged-in user
                var existingItem = _farmcontext.cart
                    .FirstOrDefault(item => item.productId == cartItem.productId && item.userId == userId);
                decimal productPrice = product.productPrice;
                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity > 0 ? cartItem.Quantity : 1;
                    existingItem.totalPrice = existingItem.Quantity * productPrice;
                    _farmcontext.cart.Update(existingItem);
                }
                else
                {
                    cartItem.userId = userId;
                    cartItem.Quantity = cartItem.Quantity > 0 ? cartItem.Quantity : 1;
                    cartItem.totalPrice = cartItem.Quantity * productPrice;
                    _farmcontext.cart.Add(cartItem);
                }
            }
            else
            {
                // Handle cart for session-based user
                var sessionId = GetOrCreateSessionId();
                var existingItem = _farmcontext.cart
                    .FirstOrDefault(item => item.productId == cartItem.productId && item.sessionId == sessionId);
                decimal productPrice = product.productPrice;
                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity > 0 ? cartItem.Quantity : 1;
                    existingItem.totalPrice = existingItem.Quantity * productPrice;
                    _farmcontext.cart.Update(existingItem);
                }
                else
                {
                    cartItem.sessionId = sessionId;
                    cartItem.Quantity = cartItem.Quantity > 0 ? cartItem.Quantity : 1;
                    cartItem.totalPrice = cartItem.Quantity * productPrice;
                    _farmcontext.cart.Add(cartItem);
                }
            }

            _farmcontext.SaveChanges();
        }

        public IEnumerable<Cart> GetCartDetails(Guid? userId)
        {
            if (userId.HasValue)
            {
                // Return cart details for a specific logged-in user
                return _farmcontext.cart.Where(c => c.userId == userId).Include(c => c.product).ToList();
            }

            // Return cart details for session-based user
            var sessionId = GetOrCreateSessionId();
            return _farmcontext.cart.Where(c => c.sessionId == sessionId).Include(c => c.product).ToList();
        }

        public void UpdateQuantity(int productId, Guid? userId,int newQuantity)
        {
           // Guid? userId = GetLoggedInUserId();
            Cart cartItem;

            if (userId.HasValue)
            {
                // Find cart item for logged-in user
                cartItem = _farmcontext.cart
                    .Include(c => c.product)
                    .FirstOrDefault(item => item.productId == productId && item.userId == userId);
            }
            else
            {
                // Find cart item for session-based user
                var sessionId = GetOrCreateSessionId();
                cartItem = _farmcontext.cart
                    .Include(c => c.product)
                    .FirstOrDefault(item => item.productId == productId && item.sessionId == sessionId);
            }

            if (cartItem == null)
            {
                throw new Exception("Cart item not found.");
            }

            if (newQuantity == 0)
            {
                // Remove item from cart if quantity is zero
                _farmcontext.cart.Remove(cartItem);
            }
            else
            {
                // Update item with new quantity and calculate total price
                decimal productPrice = cartItem.product.productPrice;
                cartItem.Quantity = newQuantity;
                cartItem.totalPrice = newQuantity * productPrice;
                _farmcontext.cart.Update(cartItem);
            }

            _farmcontext.SaveChanges();
        }

        private Guid? GetLoggedInUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : (Guid?)null;
        }

        private Guid GetOrCreateSessionId()
        {
            if (!_httpContextAccessor.HttpContext.Session.TryGetValue("SessionId", out var sessionIdBytes))
            {
                var sessionId = Guid.NewGuid();
                _httpContextAccessor.HttpContext.Session.Set("SessionId", sessionId.ToByteArray());
                return sessionId;
            }

            return new Guid(sessionIdBytes);
        }

        

        //public void AddToCart(Cart cartItem)
        //{
        //    // Retrieve or create the session ID for the current operation
        //    var sessionId = GetOrCreateSessionId();

        //    // Set the session ID for the cart item
        //    cartItem.sessionId = sessionId;

        //    // Find the product details in the database
        //    var product = _farmcontext.product.FirstOrDefault(p => p.ProductId == cartItem.productId);
        //    if (product == null)
        //    {
        //        throw new Exception("Product not found");
        //    }

        //    // Check if the item already exists in the cart for the current session
        //    var existingItem = _farmcontext.cart
        //        .FirstOrDefault(item => item.productId == cartItem.productId && item.sessionId == sessionId);

        //    decimal productPrice = product.productPrice; // Assuming productPrice is a decimal in Product

        //    if (existingItem != null)
        //    {
        //        // If the item exists, update the quantity by adding the specified amount (default to 1 if not provided)
        //        existingItem.Quantity += cartItem.Quantity > 0 ? cartItem.Quantity : 1;
        //        existingItem.totalPrice = existingItem.Quantity * productPrice; // Calculate totalPrice with decimal
        //        _farmcontext.cart.Update(existingItem);
        //    }
        //    else
        //    {
        //        // If the item does not exist, add it with the specified quantity or default to 1
        //        cartItem.sessionId = sessionId;
        //        cartItem.Quantity = cartItem.Quantity > 0 ? cartItem.Quantity : 1;
        //        cartItem.totalPrice = cartItem.Quantity * productPrice; // Calculate totalPrice with decimal
        //        cartItem.product = product; // Link the product for relational mapping
        //        _farmcontext.cart.Add(cartItem);
        //    }

        //    // Save changes to the database
        //    _farmcontext.SaveChanges();
        //}

        //private Guid GetOrCreateSessionId()
        //{
        //        // Retrieve the session ID from cookies
        //        var sessionIdCookie = _httpContextAccessor.HttpContext?.Request.Cookies["SessionId"];

        //        // If the session ID exists in the cookie, return it
        //        if (!string.IsNullOrEmpty(sessionIdCookie) && Guid.TryParse(sessionIdCookie, out Guid existingSessionId))
        //        {
        //            return existingSessionId;
        //        }

        //        // If not, create a new session ID
        //        var newSessionId = Guid.NewGuid();

        //        // Store the new session ID in a cookie
        //        _httpContextAccessor.HttpContext?.Response.Cookies.Append("SessionId", newSessionId.ToString(), new CookieOptions
        //        {
        //            Expires = DateTime.Now.AddMinutes(1) // Set expiration as needed
        //        });

        //        return newSessionId;

        //}

        //public IEnumerable<Cart> GetCartDetails()
        //{
        //    var sessionId = GetOrCreateSessionId();
        //    return _farmcontext.cart.Where(ci => ci.sessionId == sessionId).Include(obj => obj.product).ToList();
        //}

        //public void UpdateQuantity(int productId, int newQuantity)
        //{
        //    var sessionId = GetOrCreateSessionId();
        //    var cartItem = _farmcontext.cart
        //        .Include(c => c.product)
        //        .FirstOrDefault(item => item.productId == productId && item.sessionId == sessionId);

        //    if (cartItem == null)
        //    {
        //        throw new Exception("Cart item not found for this session.");
        //    }

        //    if (newQuantity == 0)
        //    {
        //        // Remove item from cart if quantity is zero
        //        _farmcontext.cart.Remove(cartItem);
        //    }
        //    else
        //    {
        //        // Update item with new quantity and calculate total price
        //        decimal productPrice = cartItem.product.productPrice;
        //        cartItem.Quantity = newQuantity;
        //        cartItem.totalPrice = newQuantity * productPrice;
        //        _farmcontext.cart.Update(cartItem);
        //    }

        //    _farmcontext.SaveChanges();
        //}


    }
}
