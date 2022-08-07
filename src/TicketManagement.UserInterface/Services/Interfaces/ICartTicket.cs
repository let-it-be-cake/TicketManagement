using System.Collections.Generic;

namespace TicketManagement.UserInterface.Services
{
    public interface ICartTicket
    {
        public void AddToCart(int cartItem);

        public void RemoveFromCart(int item);

        public void ClearCart();

        public List<int> GetAllFromCart();
    }
}
