using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TicketManagement.UserInterface.Services
{
    internal sealed class CartTicketService : ICartTicket
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartTicketService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(int cartItem)
        {
            List<int> cartItems = GetObjectFromJson("cart") ?? new List<int>();

            if (!cartItems.Contains(cartItem))
            {
                cartItems.Add(cartItem);
                SetObjectAsJson("cart", cartItems);
            }
        }

        public void RemoveFromCart(int item)
        {
            List<int> cart = GetObjectFromJson("cart");

            if (cart == null)
            {
                return;
            }

            cart.Remove(item);
            SetObjectAsJson("cart", cart);
        }

        public void ClearCart()
        {
            SetObjectAsJson("cart", null);
        }

        public List<int> GetAllFromCart()
        {
            List<int> cartCollecition = GetObjectFromJson("cart") ?? new List<int>();
            return cartCollecition;
        }

        private void SetObjectAsJson(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        private List<int> GetObjectFromJson(string key)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);
            var collection = value == null ? default(List<int>) : JsonConvert.DeserializeObject<List<int>>(value);
            return collection;
        }
    }
}
