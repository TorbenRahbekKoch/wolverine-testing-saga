using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Bootstrapping;

public static class Application
{
    public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureApplication();
        return builder;
    }
    
    public static IHostBuilder ConfigureApplication(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton<TodoData>();
        });
        return builder;
    }

}