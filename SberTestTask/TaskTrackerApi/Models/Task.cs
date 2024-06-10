namespace TaskTracker.Models
{
    /// <summary>
    /// Задача.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// Дата завершения.
        /// </summary>
        public DateTime CompleteDate { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Статус выполнения.
        /// </summary>
        public bool IsComplete { get; set; }

    }
}
