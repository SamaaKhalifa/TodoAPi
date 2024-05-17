using Microsoft.EntityFrameworkCore;

namespace TodoAPi
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions options) : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
