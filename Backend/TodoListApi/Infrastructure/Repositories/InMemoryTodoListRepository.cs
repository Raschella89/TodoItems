using System.Collections.Generic;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

public class InMemoryTodoListRepository : ITodoListRepository
{
    private int _lastId = 0;
    private readonly List<string> _categories = new() { "Work", "Personal", "Hobby" };

    public int GetNextId()
    {
        _lastId++;
        return _lastId;
    }

    public List<string> GetAllCategories()
    {
        // Return a copy so callers cannot mutate internal list
        return new List<string>(_categories);
    }
}
