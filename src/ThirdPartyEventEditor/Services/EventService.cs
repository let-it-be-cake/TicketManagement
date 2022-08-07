using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;
using ThirdPartyEventEditor.Exceptions;
using ThirdPartyEventEditor.Services.Interfaces;
using ThirdPartyEventEditor.ServiceTables;

[assembly: InternalsVisibleTo("ThirdPartyEventEditor.IntegrationTests")]

namespace ThirdPartyEventEditor.Services
{
    internal class EventService : IProxyService<Event>, IDisposable
    {
        private readonly string _jsonFilePath;
        private readonly Mutex _fileAccess = new Mutex();

        private bool _disposed;

        public EventService(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
        }

        public void Add(Event item)
        {
            CreateValidate(item);
            var items = ReadAll();
            item.Id = 0;

            if (items.Count != 0)
            {
                item.Id = items.Max(o => o.Id);
            }

            item.Id++;

            items.Add(item);
            WriteAll(items);
        }

        public void Change(Event item)
        {
            CreateValidate(item);
            List<Event> items = ReadAll().Where(o => o.Id != item.Id).ToList();
            items.Add(item);
            WriteAll(items);
        }

        public void Delete(int id)
        {
            var items = ReadAll().Where(o => o.Id != id);
            WriteAll(items);
        }

        public Event Read(int id)
        {
            var item = ReadAll().FirstOrDefault(o => o.Id == id);
            return item;
        }

        public List<Event> ReadAll()
        {
            try
            {
                _fileAccess.WaitOne();

                string serializedJson = File.ReadAllText(_jsonFilePath);
                List<Event> json = JsonConvert.DeserializeObject<List<Event>>(serializedJson)
                    ?? new List<Event>();
                return json;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _fileAccess.ReleaseMutex();
            }
        }

        private void WriteAll(IEnumerable<Event> events)
        {
            _fileAccess.WaitOne();

            try
            {
                string json = JsonConvert.SerializeObject(events);
                File.WriteAllText(_jsonFilePath, json);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _fileAccess.ReleaseMutex();
            }
        }

        private void CreateValidate(Event item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot be null");
            }

            if (item.StartDate > item.EndDate)
            {
                throw new EventDateTimeException("The event ended before it started.");
            }

            if (item.EndDate < DateTime.UtcNow)
            {
                throw new EventDateTimeException("The event has already ended.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fileAccess?.Dispose();
            }

            _disposed = true;
        }
    }
}