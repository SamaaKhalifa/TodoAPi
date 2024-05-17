using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoAPi;

var builder = WebApplication.CreateBuilder(args);

//Register Services through DI
builder.Services.AddDbContext<TodoDb>(opt => 
opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

//Configure Pipline- Use/Map Method

//GetAllItems
app.MapGet("/todoitems",async(TodoDb db)=>
await db.TodoItems.ToListAsync());
//GetItemById
app.MapGet("/todoitems/{id}", async (int id,TodoDb db) =>
await db.TodoItems.FindAsync(id));
//Create New Item
app.MapPost("/todoitems", async (TodoItem nwItem, TodoDb db) =>
{
    db.TodoItems.Add(nwItem);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{nwItem.Id}", nwItem);
});
//Update Item
app.MapPut("/todoitems/{id}", async (int id,TodoItem updatedItem, TodoDb db) =>
{
    var oldItem= await db.TodoItems.FindAsync(id);
    if(oldItem != null)
    {
        oldItem.Title = updatedItem.Title;
        oldItem.IsDone=updatedItem.IsDone;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
}
);
//Delete Item
app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) => 
{
    var oldItem = await db.TodoItems.FindAsync(id);
    if (oldItem != null)
    {
        db.TodoItems.Remove(oldItem);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
}
);
app.Run();
