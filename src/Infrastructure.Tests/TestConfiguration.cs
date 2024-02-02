using Application;
using Infrastructure.Bootstrapping;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;

namespace Infrastructure.Tests;

public class TestConfiguration
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(
        new WebApplicationOptions()
        {
            WebRootPath = Directory.GetCurrentDirectory(),
            ContentRootPath = Directory.GetCurrentDirectory()
        });

        builder
            .Host
            .ConfigureApplication()
            .ConfigureWolverineForTest();
        
        // Register the client messenger mock, add it as singleton
        // to ensure the state inside it can be tested
        builder.Services.AddSingleton<IClientMessenger, TestClientMessenger>();
        
        var app = builder.Build();
        app.Run();
    }
}

public static class WolverineConfiguration
{
    public static IHostBuilder ConfigureWolverineForTest(this IHostBuilder builder)
    {
        return builder.UseWolverine(options =>
        {
            options.ApplicationAssembly = typeof(WolverineConfiguration).Assembly;
            options.Discovery.IncludeAssembly(typeof(DomainEventHandler).Assembly);
            options.Discovery.IncludeAssembly(typeof(TodoItemCreated).Assembly);
            options.LocalQueueFor<CreateTodoItem>()
                .Sequential()
                .MaximumParallelMessages(1);
            // The documentation talks about the DurabilityMode.MediatorOnly
            // However, it is not clear what it does. Enabling it causes
            // things to stop working
            //options.Durability.Mode = DurabilityMode.MediatorOnly;
            Console.WriteLine(options.DescribeHandlerMatch(typeof(DomainEventHandler)));
        });
    }    
}