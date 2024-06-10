using Microsoft.EntityFrameworkCore;

namespace TaskTracker
{
    /// <summary>
    /// Данные трекера задач.
    /// </summary>
    public class TaskTrackerContext : DbContext
    {
        /// <summary>
        /// Подключение.
        /// </summary>
        /// <param name="options"></param>
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Данные о задачах.
        /// </summary>
        public DbSet<Models.Task> Tasks { get; set; } = null!;
    }
}
