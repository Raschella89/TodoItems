using System.Collections.Generic;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

public class InMemoryTodoListRepository : ITodoListRepository
{
    private int _next = 1;
    public int GetNextId() => _next++;
    public List<string> GetAllCategories() => new() { "Work", "Personal", "Hobby" };
}