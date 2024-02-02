using Application;
using Wolverine.Attributes;

namespace Infrastructure.Messaging;
[WolverineHandler]
public static class CommandHandler
{
    [WolverineHandler]
    public static IList<IDomainEvent> HandleCreateTodoItem(
        CreateTodoItem todoItem,
        TodoData todoData)
    {
        var tryCreateTodoItem = new TryCreateTodoItem(todoItem.WhatToDo);
        return tryCreateTodoItem.Execute(todoData);
    }
}