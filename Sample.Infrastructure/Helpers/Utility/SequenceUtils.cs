using Dapper;
using Sample.Infrastructure.Common;
using Sample.Infrastructure.Helpers.Configuration;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Helpers.Utility
{
    public static class SequenceUtils
    {
        public static decimal GetNextSequenceValue(string sequenceName)
        {
            string connectionString = ConfigurationHelper.GetConnectionString();

            using (IDbConnection dbConnection = new OracleConnection(connectionString))
            {
                dbConnection.Open();

                // Use Dapper to execute the query and retrieve the next value from the sequence.
                var parameters = new DynamicParameters();
                parameters.Add("result", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                dbConnection.Execute($"BEGIN :result := {sequenceName}.NEXTVAL; END;", parameters);

                return parameters.Get<decimal>("result");
            }
        }
    }
}
