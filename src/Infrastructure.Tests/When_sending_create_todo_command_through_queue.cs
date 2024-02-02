using FluentAssertions;
using Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Infrastructure.Tests;

public class When_sending_create_todo_command_through_queue
    (TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>
{
    [Fact]
    public async void Verify_that_item_created_event_is_sent_back()
    {
        var messageBus = factory.Services.GetService<IMessageBus>();

        var createTodoCommand = new CreateTodoItem("Remember to check return result!");
        await messageBus.InvokeAsync(createTodoCommand);

        // Wait "some time" and expect Wolverine to be done handling
        // all messages... 
        
        await Task.Delay(500);

        var messenger = factory.Services.GetService<IClientMessenger>() as TestClientMessenger;

        messenger.Events.Should().HaveCount(1);
    }
}