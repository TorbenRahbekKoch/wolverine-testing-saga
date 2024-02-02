namespace Application;

public readonly record struct TodoItemCreated(
    ToDoItem WhatToDo)
    : IDomainEvent;