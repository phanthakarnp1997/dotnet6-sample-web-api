using Sample.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Helpers.Configuration
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot _configuration;
        private static bool _isInitialized = false;

        public static void Initialize(IConfigurationRoot configuration)
        {
            if (!_isInitialized)
            {
                _configuration = configuration;
                _isInitialized = true;
            }
        }

        public static string GetConnectionString()
        {
            return _configuration.GetConnectionString(Constants.ConnectionName);
        }
    }

}
