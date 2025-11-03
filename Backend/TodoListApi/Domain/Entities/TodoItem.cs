using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Domain.Entities;

public class TodoItem
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public List<Progression> Progressions { get; private set; } = new();

    public bool IsCompleted => CumulativePercent >= 100m;

    public TodoItem(int id, string title, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required", nameof(title));
        Id = id;
        Title = title;
        Description = description ?? string.Empty;
        Category = category ?? string.Empty;
    }

    public decimal CumulativePercent => Progressions.Sum(p => p.Percent);

    public void AddProgression(DateTime date, decimal percent)
    {
        // Validate percent >0 and <100
        if (percent <= 0m || percent >= 100m)
            throw new InvalidOperationException("Percent must be > 0 and < 100.");

        // Date must be strictly later than last progression (if any)
        if (Progressions.Any() && date <= Progressions.Max(p => p.Date))
            throw new InvalidOperationException("New progression date must be later than previous ones.");

        // Ensure cumulative percent <= 100
        if (CumulativePercent + percent > 100m)
            throw new InvalidOperationException("Total progress cannot exceed 100%.");

        Progressions.Add(new Progression(date, percent));
    }

    public void UpdateDescription(string newDescription)
    {
        if (CumulativePercent > 50m) throw new InvalidOperationException("Cannot update item with more than 50% progress.");
        Description = newDescription ?? string.Empty;
    }

    public void EnsureRemovable()
    {
        if (CumulativePercent > 50m) throw new InvalidOperationException("Cannot remove item with more than 50% progress.");
    }
}
