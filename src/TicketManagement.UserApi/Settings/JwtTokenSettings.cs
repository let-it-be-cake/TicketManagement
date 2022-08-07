namespace TicketManagement.UserApi.Settings
{
    public class JwtTokenSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public int LifeTime { get; set; }
    }
}
