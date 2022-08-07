using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.UnitTests.ServicesTesting
{
    public class SessionMock : ISession
    {
        private readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();

        public bool IsAvailable => true;

        public string Id => "";

        public IEnumerable<string> Keys => new List<string>();

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = Encoding.UTF8.GetString(value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage.TryGetValue(key, out object obj))
            {
                value = Encoding.ASCII.GetBytes(obj.ToString());
                return true;
            }

            value = null;
            return false;
        }
    }
}
