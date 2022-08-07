using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.DataAccess.Repositories.Ado
{
    internal class AdoEventRepository : IRepository<Event>
    {
        /// <inheritdoc cref="IRepository{T}"/>
        private readonly string _connectionString;

        public AdoEventRepository(string connection)
        {
            _connectionString = connection;
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task CreateAsync(Event item)
        {
            var query = @"EXEC [dbo].sp_CreateEvent 
                @name = @NameValue, 
                @description = @DescriptionValue, 
                @layoutId = @LayoutIdValue,
                @dateTimeStart = @StartValue,
                @dateTimeEnd = @EndValue,
                @imageUrl = @ImageUrlValue,
                @eventId = @EventIdValue OUTPUT";

            var outParam = new SqlParameter
            {
                ParameterName = "@EventIdValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };

            using var sqlCommand = new SqlCommand(query);

            sqlCommand.Parameters.Add(outParam);
            sqlCommand.Parameters.AddWithValue("@NameValue", item.Name);
            sqlCommand.Parameters.AddWithValue("@DescriptionValue", item.Description);
            sqlCommand.Parameters.AddWithValue("@LayoutIdValue", item.LayoutId);
            sqlCommand.Parameters.AddWithValue("@StartValue", item.DateTimeStart);
            sqlCommand.Parameters.AddWithValue("@EndValue", item.DateTimeEnd);
            sqlCommand.Parameters.AddWithValue("@ImageUrlValue", item.ImageUrl);

            using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;

            await sqlCommand.ExecuteNonQueryAsync();

            item.Id = (int)outParam.Value;
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task DeleteAsync(int id)
        {
            var query = @"EXEC [dbo].sp_DeleteEvent @id = @IdValue;";

            using var sqlCommand = new SqlCommand(query);

            sqlCommand.Parameters.AddWithValue("@IdValue", id);

            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;
            await sqlCommand.ExecuteNonQueryAsync();
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task<Event> GetAsync(int id)
        {
            var query = @"EXEC [dbo].sp_GetEvent @id = @IdValue;";
            using var sqlCommand = new SqlCommand(query);

            sqlCommand.Parameters.AddWithValue("@IdValue", id);

            using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            bool canRead = await reader.ReadAsync();

            if (!reader.HasRows || !canRead)
            {
                return null;
            }

            var output = new Event
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Description = (string)reader["Description"],
                DateTimeStart = (DateTime)reader["DateTimeStart"],
                DateTimeEnd = (DateTime)reader["DateTimeEnd"],
                LayoutId = (int)reader["LayoutId"],
                ImageUrl = (string)reader["ImageUrl"],
            };

            return output;
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public IQueryable<Event> GetAll()
        {
            var query = @"EXEC [dbo].sp_GetAllEvents";

            using var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            using var sqlCommand = new SqlCommand(query)
            {
                Connection = sqlConnection,
            };

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            var eventList = new List<Event>();

            while (reader.Read())
            {
                var @event = new Event
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    DateTimeStart = (DateTime)reader["DateTimeStart"],
                    DateTimeEnd = (DateTime)reader["DateTimeEnd"],
                    LayoutId = (int)reader["LayoutId"],
                    ImageUrl = (string)reader["ImageUrl"],
                };

                eventList.Add(@event);
            }

            return eventList.AsQueryable();
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task UpdateAsync(Event item)
        {
            var query = @"EXEC [dbo].sp_UpdateEvent
                        @id = @IdValue, 
                        @name = @NameValue, 
                        @description = @DescriptionValue, 
                        @layoutId = @LayoutIdValue,
                        @dateTimeStart = @StartValue,
                        @dateTimeEnd = @EndValue,
                        @imageUrl = @ImageUrlValue;";

            using var sqlCommand = new SqlCommand(query);

            sqlCommand.Parameters.AddWithValue("@IdValue", item.Id);
            sqlCommand.Parameters.AddWithValue("@NameValue", item.Name);
            sqlCommand.Parameters.AddWithValue("@DescriptionValue", item.Description);
            sqlCommand.Parameters.AddWithValue("@LayoutIdValue", item.LayoutId);
            sqlCommand.Parameters.AddWithValue("@StartValue", item.DateTimeStart);
            sqlCommand.Parameters.AddWithValue("@EndValue", item.DateTimeEnd);
            sqlCommand.Parameters.AddWithValue("@ImageUrlValue", item.ImageUrl);

            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;
            await sqlCommand.ExecuteNonQueryAsync();
        }
    }
}