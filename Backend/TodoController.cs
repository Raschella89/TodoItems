using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TodoApp.Application.Services;

namespace TodoApp.WebApi.Controllers;

[ApiController]
[Route("api/todolist")]
public class TodoController : ControllerBase
{
    private readonly TodoListService _service;
    private readonly Domain.Interfaces.ITodoListRepository _repo;

    public TodoController(TodoListService service, Domain.Interfaces.ITodoListRepository repo)
    {
        _service = service;
        _repo = repo;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _service.GetAllItems()
            .Select(i => new
            {
                i.Id,
                i.Title,
                i.Description,
                i.Category,
                IsCompleted = i.IsCompleted,
                Progressions = i.Progressions.Select(p => new { p.Date, p.Percent })
            });
        return Ok(items);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTodoDto dto)
    {
        var id = _repo.GetNextId();
        _service.AddItem(id, dto.Title, dto.Description, dto.Category);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateTodoDto dto)
    {
        _service.UpdateItem(id, dto.Description);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.RemoveItem(id);
        return NoContent();
    }

    [HttpPost("{id:int}/progress")]
    public IActionResult AddProgression(int id, [FromBody] AddProgressDto dto)
    {
        // Use server time only if dto.Date is not provided; here allow optional date
        var when = dto.Date ?? DateTime.UtcNow;
        _service.RegisterProgression(id, when, dto.Percent);
        return Accepted();
    }
}

public record CreateTodoDto(string Title, string Description, string Category);
public record UpdateTodoDto(string Description);
public record AddProgressDto(decimal Percent, DateTime? Date);