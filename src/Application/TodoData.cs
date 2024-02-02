namespace Application;

public class TodoData
{
    internal readonly List<ToDoItem> todoItems = new();

    public IReadOnlyList<ToDoItem> ToDoItems => todoItems;
}