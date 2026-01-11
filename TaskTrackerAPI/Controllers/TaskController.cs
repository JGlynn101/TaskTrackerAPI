using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Contracts.Tasks;
using TaskTrackerAPI.Services;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _taskService.GetAll();
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var tasks = _taskService.GetTask(id);
            return Ok(tasks);
        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTask = _taskService.Add(request.Name, request.Description);

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdTask.Id },
                    createdTask
            );
        }

        [HttpPut]
        public IActionResult UpdateTask()
        {
            return Ok("Controller works");
        }

        [HttpDelete]
        public IActionResult DeleteTask()
        {
            return Ok("Controller works");
        }
    }
}
