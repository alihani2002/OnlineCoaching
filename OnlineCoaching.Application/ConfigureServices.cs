using Microsoft.Extensions.DependencyInjection;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            return services;

        }
    }
}
