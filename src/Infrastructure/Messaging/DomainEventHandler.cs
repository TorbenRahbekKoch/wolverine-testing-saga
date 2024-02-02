using Application;
using Wolverine.Attributes;

namespace Infrastructure.Messaging;

[WolverineHandler]
public static class DomainEventHandler
{
    /// <summary>
    /// I would like to simply use IDomainEvent as the message type, but
    /// Wolverine doesn't allow for that. Only concrete types, not event
    /// a base type will work.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="messenger"></param>
    [WolverineHandler]
    public static void Handle(TodoItemCreated message, IClientMessenger messenger)
    {
        messenger.SendDomainEvent(message);
    }
}