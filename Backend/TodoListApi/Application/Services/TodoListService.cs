using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

public class TodoListService : ITodoList
{
    private readonly ITodoListRepository _repo;

    private readonly List<TodoItem> _items = new();

    public TodoListService(ITodoListRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public void AddItem(int id, string title, string description, string category)
    {
        if (!_repo.GetAllCategories().Contains(category))
            throw new InvalidOperationException("Invalid category.");
        if (_items.Any(i => i.Id == id)) throw new InvalidOperationException("Id already exists.");
        var item = new TodoItem(id, title, description, category);
        _items.Add(item);
    }

    public void UpdateItem(int id, string description)
    {
        var item = Find(id);
        item.UpdateDescription(description);
    }

    public void RemoveItem(int id)
    {
        var item = Find(id);
        item.EnsureRemovable();
        _items.Remove(item);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = Find(id);
        item.AddProgression(dateTime, percent);
    }

    public IEnumerable<TodoItem> GetAllItems()
    {
        return _items.OrderBy(i => i.Id).ToList();
    }

    public void PrintItems()
    {
        foreach (var i in _items.OrderBy(x => x.Id))
        {
            Console.WriteLine($"{i.Id}) {i.Title} - {i.Description} ({i.Category}) Completed:{i.IsCompleted}");
            decimal acc = 0;
            foreach (var p in i.Progressions)
            {
                acc += p.Percent;
                int bars = (int)Math.Round(acc / 2); 
                var bar = new string('O', bars).PadRight(50);
                Console.WriteLine($"{p.Date} - {acc}%     |{bar}|");
            }
        }
    }

    private TodoItem Find(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) throw new KeyNotFoundException($"TodoItem {id} not found.");
        return item;
    }
}