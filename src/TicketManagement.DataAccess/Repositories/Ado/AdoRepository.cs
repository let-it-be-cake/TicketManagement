using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.Entities.Interfaces;

namespace TicketManagement.DataAccess.Repositories.Ado
{
    internal class AdoRepository<T> : IRepository<T>
        where T : class, IHasId, new()
    {
        private readonly List<PropertyInfo> _properties;
        private readonly string _connectionString;

        private string _table;

        public AdoRepository(string connectionString)
        {
            _connectionString = connectionString;
            _properties = new List<PropertyInfo>(typeof(T).GetProperties());
            Table = typeof(T).Name;
        }

        /// <summary>
        /// Get a table to write to.
        /// </summary>
        public string Table
        {
            get => _table;

            private set
            {
                if (value.Contains("]") || value.Contains("[") || value.Contains(" "))
                {
                    throw new ArgumentException("The incoming argument" +
                        "cannot contain square brackets or spaces.");
                }

                _table = " [" + value + "] ";
            }
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task CreateAsync(T item)
        {
            var preQuery = new StringBuilder();
            var query = new StringBuilder();

            preQuery.Append(@";INSERT INTO ");
            preQuery.Append(Table);
            preQuery.Append(@" (");

            query.Append(@"VALUES ");
            query.Append(@" (");

            foreach (PropertyInfo property in _properties)
            {
                if (property.Name.ToLower() == "id" || property.GetValue(item) == null)
                {
                    continue;
                }

                preQuery.Append(property.Name);
                preQuery.Append(",");

                query.Append("@");
                query.Append(property.Name);
                query.Append(@"Value ,");
            }

            preQuery.Remove(preQuery.Length - 1, 1);

            preQuery.Append(")");

            query = query.Remove(query.Length - 1, 1);
            query.Append(")");
            query = preQuery.Append(query);

            query.Append(" SELECT SCOPE_IDENTITY()");

            using var sqlCommand = new SqlCommand(query.ToString());

            foreach (PropertyInfo property in _properties)
            {
                if (property.Name.ToLower() == "id" || property.GetValue(item) == null)
                {
                    continue;
                }

                sqlCommand.Parameters.AddWithValue(
                    "@" + property.Name + "Value",
                    property.GetValue(item));
            }

            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;
            item.Id = (int)(decimal)await sqlCommand.ExecuteScalarAsync();
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task DeleteAsync(int id)
        {
            var query = new StringBuilder();
            query.Append(@"; DELETE FROM ");
            query.Append(Table);
            query.Append(@" WHERE ");

            PropertyInfo property = _properties.First(o => o.Name.ToLower() == "id");

            query.Append(property.Name);
            query.Append(@"=@" + property.Name + "Value;");

            using var sqlCommand = new SqlCommand(query.ToString());
            sqlCommand.Parameters.AddWithValue("@" + property.Name + "Value", id);

            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;
            await sqlCommand.ExecuteNonQueryAsync();
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task<T> GetAsync(int id)
        {
            T item = new T();
            var query = new StringBuilder();
            query.Append(@"SELECT ");

            foreach (PropertyInfo property in _properties)
            {
                query.Append(property.Name + ",");
            }

            query.Remove(query.Length - 1, 1);
            query.Append(" FROM " + Table + " WHERE Id = @idValue;");

            using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();

            using var sqlCommand = new SqlCommand(query.ToString());
            sqlCommand.Parameters.AddWithValue("@idValue", id);
            sqlCommand.Connection = sqlConnection;

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            bool canRead = reader.Read();

            if (!reader.HasRows || !canRead)
            {
                return null;
            }

            int count = reader.FieldCount;

            for (int i = 0; i < count; i++)
            {
                string fieldName = reader.GetName(i);
                PropertyInfo propInfo = typeof(T).GetProperty(fieldName);
                propInfo?.SetValue(item, reader.GetValue(i));
            }

            return item;
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public IQueryable<T> GetAll()
        {
            var query = new StringBuilder(@"SELECT ");

            foreach (PropertyInfo property in _properties)
            {
                query.Append(property.Name + ",");
            }

            query.Remove(query.Length - 1, 1);

            query.Append(" FROM " + Table);

            var list = new List<T>();
            var item = new T();
            Type typeOfT = typeof(T);

            using var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            using var sqlCommand = new SqlCommand(query.ToString())
            {
                Connection = sqlConnection,
            };

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            if (!reader.HasRows)
            {
                return list.AsQueryable();
            }

            bool canRead = reader.Read();

            while (canRead)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i) == DBNull.Value)
                    {
                        continue;
                    }

                    string fieldName = reader.GetName(i);
                    PropertyInfo propInfo = typeOfT.GetProperty(fieldName);
                    propInfo?.SetValue(item, reader.GetValue(i));
                }

                list.Add(item);
                item = new T();
                canRead = reader.Read();
            }

            return list.AsQueryable();
        }

        /// <inheritdoc cref="IRepository{T}"/>
        public async Task UpdateAsync(T item)
        {
            var query = new StringBuilder();
            query.Append("UPDATE ");
            query.Append(Table);
            query.Append(" SET ");

            string idName = string.Empty;
            object idValue = null;

            foreach (PropertyInfo property in _properties)
            {
                if (property.Name == "Id")
                {
                    idName = property.Name;
                    idValue = property.GetValue(item);
                    continue;
                }

                query.Append("[" + property.Name + "]=@" + property.Name + "Value, ");
            }

            query = query.Remove(query.Length - 2, 2);
            query.Append(" WHERE [" + idName + "]=" + idValue + ";");

            using var sqlCommand = new SqlCommand(query.ToString());

            foreach (PropertyInfo property in _properties)
            {
                object value = property.GetValue(item).ToString();
                sqlCommand.Parameters.AddWithValue("@" + property.Name + "Value", value);
            }

            using var sqlConnection = new SqlConnection(_connectionString);

            await sqlConnection.OpenAsync();
            sqlCommand.Connection = sqlConnection;
            await sqlCommand.ExecuteNonQueryAsync();
        }
    }
}