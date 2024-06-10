using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// ���������� �����.
    /// </summary>
    [ApiController]
    [Route("TaskTracker/api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly TaskTrackerContext _context;

        /// <summary>
        /// ������� ����������.
        /// </summary>
        /// <param name="logger">�����������.</param>
        /// <param name="context">���� ������.</param>
        public TasksController(ILogger<TasksController> logger, TaskTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// ������� ������.
        /// </summary>
        /// <param name="userTask">������ �� ������������.</param>
        /// <returns>����� ������.</returns>
        /// <response code="201">������ �������.</response>
        /// <response code="400">������������ ������ ������������.</response>
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
        /// �������� ������ ���� �����.
        /// </summary>
        /// <returns>��� ������.</returns>
        /// <response code="200">������ �������.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            return Ok(await _context.Tasks.ToListAsync());
        }

        /// <summary>
        /// �������� ������ �� ��������������.
        /// </summary>
        /// <param name="id">������������� ������.</param>
        /// <returns>������ � <paramref name="id"/>.</returns>
        /// <response code="200">������ ��������.</response>
        /// <response code="404">������ �� �������.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            return task == null
                ? NotFound()
                : Ok(task);
        }

        /// <summary>
        /// ��������� ������ ������.
        /// </summary>
        /// <param name="id">������������� ������.</param>
        /// <param name="newTask">����������� ������.</param>
        /// <returns>����������� ������.</returns>
        /// <response code="200">������ ���������.</response>
        /// <response code="400"><paramref name="id"/> �� ������������ �������������� <paramref name="newTask"/>. </response>
        /// <response code="404">������ �� �������.</response>
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
        /// ������������� ������ ������ �� ���������.
        /// </summary>
        /// <param name="id">������������� ������.</param>
        /// <returns>����������� ������.</returns>
        /// <response code="200">������ ���������.</response>
        /// <response code="404">������ �� �������.</response>
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
        /// ������� ������.
        /// </summary>
        /// <param name="id">������������� ������.</param>
        /// <response code="204">������ �������.</response>
        /// <response code="404">������ �� �������.</response>
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
