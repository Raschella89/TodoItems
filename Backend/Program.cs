using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TodoApp.Application.Services;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddSingleton<ITodoListRepository, InMemoryTodoListRepository>();
builder.Services.AddSingleton<TodoListService>();
builder.Services.AddSingleton<ITodoList>(sp => sp.GetRequiredService<TodoListService>());

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API V1");
    });
}

app.UseRouting();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.Run();
