using Microsoft.Extensions.Configuration;

namespace TicketManagement.UserInterface.Services
{
    internal class PagingValidationService : IPagingValidation
    {
        private readonly IConfiguration _configuration;

        public PagingValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int MaxElementOnPage(int elements)
        {
            int maxElementsOnPage = int.Parse(_configuration["MaxElementsOnPage"]);
            int defaultElementsOnPage = int.Parse(_configuration["DefaultElementsOnPage"]);

            if (elements > maxElementsOnPage)
            {
                elements = defaultElementsOnPage;
            }

            return elements;
        }
    }
}
