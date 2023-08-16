using System.Reflection;

namespace Sample.WebAPI.Config
{
    public static class AssemblyConfig
    {
        public static void RegisterAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly repositoryAssembly = Assembly.Load("Sample.Infrastructure");
            // Automatically scan and register repositories from the specified assembly and namespace
            services.Scan(scan => scan
                .FromAssemblies(repositoryAssembly) // Specify the correct assembly where your repositories are located
                .AddClasses(@class =>
                    @class.Where(type =>
                    (!type.Name.StartsWith('I')
                        && type.Name.EndsWith("Repository")) || type.Name.EndsWith("Service")
                    )
                )
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }
    }
}
