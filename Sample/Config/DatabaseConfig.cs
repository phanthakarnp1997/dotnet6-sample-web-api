using Sample.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace Sample.WebAPI.Config
{
    public static class DatabaseConfig
    {
        public static void SetupDb(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(Constants.ConnectionName);
            services.AddScoped<IDbConnection>(provider => new OracleConnection(connectionString));
        }
    }
}
