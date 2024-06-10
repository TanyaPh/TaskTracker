namespace TaskTracker.Models
{
    /// <summary>
    /// Задача от пользователя.
    /// </summary>
    public class TaskDTO
    {
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
    }
}
