using Dapper;
using Sample.Infrastructure.Common;
using Sample.Infrastructure.Helpers.Configuration;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sample.Infrastructure.Helpers.Utility
{
    public static class DateUtils
    {
        public static DateTime GetCurrentDate()
        {
            string connectionString = ConfigurationHelper.GetConnectionString();

            using (IDbConnection dbConnection = new OracleConnection(connectionString))
            {
                dbConnection.Open();

                // Execute the SQL query to retrieve the current date from the Oracle database
                var currentDate = dbConnection.QueryFirstOrDefault<DateTime>("SELECT SYSDATE FROM DUAL");

                return currentDate;
            }
        }
    }
}
