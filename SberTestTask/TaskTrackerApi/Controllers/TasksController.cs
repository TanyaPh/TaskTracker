using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// Обработчик задач.
    /// </summary>
    [ApiController]
    [Route("TaskTracker/api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly TaskTrackerContext _context;

        /// <summary>
        /// Создает обработчик.
        /// </summary>
        /// <param name="logger">Логирование.</param>
        /// <param name="context">База данных.</param>
        public TasksController(ILogger<TasksController> logger, TaskTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Создает задачу.
        /// </summary>
        /// <param name="userTask">Задача от пользователя.</param>
        /// <returns>Новая задача.</returns>
        /// <response code="201">Задача создана.</response>
        /// <response code="400">Некорректная задача пользователя.</response>
        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.TaskDTO userTask)
        {
            if (userTask == null)
            {
                return BadRequest();
            }

            var task = new Models.Task
            {
                Id = Guid.NewGuid(),
                Type = userTask.Type,
                CompleteDate = userTask.CompleteDate,
                Description = userTask.Description,
                IsComplete = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
        }

        /// <summary>
        /// Получает массив всех задач.
        /// </summary>
        /// <returns>Все задачи.</returns>
        /// <response code="200">Массив получен.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            return Ok(await _context.Tasks.ToListAsync());
        }

        /// <summary>
        /// Получает задачу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns>Задача с <paramref name="id"/>.</returns>
        /// <response code="200">Задача получена.</response>
        /// <response code="404">Задача не найдена.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            return task == null
                ? NotFound()
                : Ok(task);
        }

        /// <summary>
        /// Обновляет данные задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <param name="newTask">Обновленная задача.</param>
        /// <returns>Обновленная задача.</returns>
        /// <response code="200">Задача обновлена.</response>
        /// <response code="400"><paramref name="id"/> не соответсвует идентификатору <paramref name="newTask"/>. </response>
        /// <response code="404">Задача не найдена.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, Models.Task newTask)
        {
            if (id != newTask.Id)
            {
                return BadRequest();
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Type = newTask.Type;
            task.CompleteDate = newTask.CompleteDate;
            task.Description = newTask.Description;
            task.IsComplete = newTask.IsComplete;
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>
        /// Устанавливает статус задачи на выполнена.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns>Обновленная задача.</returns>
        /// <response code="200">Задача обновлена.</response>
        /// <response code="404">Задача не найдена.</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskStatusToComplete(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.IsComplete = true;
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>
        /// Удаляет задачу.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <response code="204">Задача удалена.</response>
        /// <response code="404">Задача не найдена.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
