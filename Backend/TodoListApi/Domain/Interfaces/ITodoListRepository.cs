using System.Collections.Generic;

namespace TodoApp.Domain.Interfaces;

public interface ITodoListRepository
{
    int GetNextId();
    List<string> GetAllCategories();
}