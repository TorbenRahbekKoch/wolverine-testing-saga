using Application;
using Infrastructure.Messaging;

namespace Infrastructure.Tests;

public class TestClientMessenger : IClientMessenger
{
    public List<IDomainEvent> Events = [];
    
    public void SendDomainEvent(IDomainEvent domainEvent)
    {
        Events.Add(domainEvent);    
    }
}