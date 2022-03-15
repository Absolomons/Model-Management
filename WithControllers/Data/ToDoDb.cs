using Microsoft.EntityFrameworkCore;
using MM.Models;

namespace MM.Data
{
    public class MMDb : DbContext
    {
        public MMDb(DbContextOptions<MMDb> options)
            : base(options) { }

        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Expense> Expenses => Set<Expense>();
    }
}
