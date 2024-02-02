using Application;

namespace Infrastructure.Messaging;

public interface IClientMessenger
{
    void SendDomainEvent(IDomainEvent domainEvent);
}