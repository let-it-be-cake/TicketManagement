using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TicketManagement.UserInterface.Models;

namespace TicketManagement.UserInterface.Helper
{
    public class LocalSaveThirdPartyEvents : IThirdPartyEventHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _key = "ThirdPartyEvents";

        public LocalSaveThirdPartyEvents(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ThirdPartyEvent> GetEvents()
        {
            List<ThirdPartyEvent> events = GetObjectFromJson(_key);
            return events;
        }

        public void SaveEvents(IEnumerable<ThirdPartyEvent> events)
        {
            SetObjectAsJson(_key, events);
        }

        private void SetObjectAsJson(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        private List<ThirdPartyEvent> GetObjectFromJson(string key)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);
            var collection =
                value == null ? new List<ThirdPartyEvent>() : JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(value);
            return collection;
        }
    }
}