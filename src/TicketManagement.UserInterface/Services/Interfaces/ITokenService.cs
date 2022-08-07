namespace TicketManagement.UserInterface.Services
{
    public interface ITokenService
    {
        public string GetToken();

        public void SetToken(string token);

        public void ResetToken();
    }
}
