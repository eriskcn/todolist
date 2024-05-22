using Microsoft.EntityFrameworkCore;
using todolist.Models;

namespace todolist.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.TaskFiles)
                .WithOne(tf => tf.TaskItem)
                .HasForeignKey(tf => tf.TaskID)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
