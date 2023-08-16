using Dapper;
using Sample.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Helpers.Utility
{
    public class DapperUtils
    {
        public static int InsertNonNullProperties<T>(IDbConnection connection, string tableName, T data)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var type = typeof(T);
            var properties = type.GetProperties();

            var nonNullProperties = properties
                .Where(prop => prop.GetValue(data) != null)
                .ToList();

            if (nonNullProperties.Count == 0)
                throw new ArgumentException("Data object does not have any non-null properties to insert.", nameof(data));

            var columnNames = nonNullProperties.Select(prop => prop.Name.ToSnakeCase().ToUpper());
            var parameterNames = nonNullProperties.Select(prop => $":{prop.Name}");

            var insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) " +
                              $"VALUES ({string.Join(", ", parameterNames)})";

            return connection.Execute(insertQuery, data);
        }
    }
}
