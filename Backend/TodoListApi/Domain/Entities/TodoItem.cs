using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Domain.Entities;

public class TodoItem
{
    public int Id { get; private set; }
    public string Title { get; private set; } = "";
    public string Description { get; private set; } = "";
    public string Category { get; private set; } = "";
    public List<Progression> Progressions { get; private set; } = new();

    public bool IsCompleted => Progressions.Sum(p => p.Percent) >= 100;

    public TodoItem(int id, string title, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required", nameof(title));
        Id = id;
        Title = title;
        Description = description ?? "";
        Category = category ?? "";
    }

    public decimal CumulativePercent => Progressions.Sum(p => p.Percent);

    public void AddProgression(DateTime date, decimal percent)
    {
        if (percent <= 0 || percent >= 100) throw new InvalidOperationException("Percent must be >0 and <100.");
        if (Progressions.Any() && date <= Progressions.Max(p => p.Date)) 
            throw new InvalidOperationException("New progression date must be later than previous ones.");

        if (CumulativePercent + percent > 100) throw new InvalidOperationException("Total progress cannot exceed 100%.");

        Progressions.Add(new Progression(date, percent));
    }

    public void UpdateDescription(string newDescription)
    {
        if (CumulativePercent > 50) throw new InvalidOperationException("Cannot update item with more than 50% progress.");
        Description = newDescription ?? "";
    }

    public void EnsureRemovable()
    {
        if (CumulativePercent > 50) throw new InvalidOperationException("Cannot remove item with more than 50% progress.");
    }
}