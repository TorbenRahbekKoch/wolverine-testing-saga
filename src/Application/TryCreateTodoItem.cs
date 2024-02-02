namespace Application;

public readonly record struct TryCreateTodoItem(
    string WhatToDo)
{
    public IList<IDomainEvent> Execute(TodoData todo)
    {
        var events = new List<IDomainEvent>();

        var newTodoItem = new ToDoItem(WhatToDo);
        todo.todoItems.Add(newTodoItem);
        events.Add(new TodoItemCreated(newTodoItem));
        return events;
    }
}