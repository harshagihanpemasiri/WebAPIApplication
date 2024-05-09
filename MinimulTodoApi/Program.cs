using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MinimulTodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(option => option.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/todoItems", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapGet("/todoItems/complete", async (TodoDb db) => await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoItems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


/*RouteGroupBuilder todos = app.MapGroup("/todo");

todos.MapGet("/", GetAllTodos);
todos.MapGet("/complete", GetCompleteTodos);
todos.MapGet("/{id}", GetTodo);
todos.MapPost("/", CreateTodo);
todos.MapPut("/{id}", UpdateTodo);
todos.MapDelete("/{id}", DeleteTodo);
*/

app.Run();

/*static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo ? TypedResults.Ok(new TodoDTO(todo)) : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoDb db, TodoDTO todoDTO)
{
    var todo = new Todo
    {
        IsComplete = todoDTO.IsComplete,
        Name = todoDTO.Name,
    };

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    todoDTO = new TodoDTO(todo);

    return TypedResults.Created($"/todoitems/{todo.Id}", todoDTO);
}

static async Task<IResult> UpdateTodo(int id, TodoDb db, TodoDTO todoDTO)
{
    var todo = await db.Todos.FindAsync(id);
     if (todo is null) return TypedResults.NotFound();

    todoDTO.Name = todo.Name;
    todoDTO.IsComplete = todo.IsComplete;
    
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}*/