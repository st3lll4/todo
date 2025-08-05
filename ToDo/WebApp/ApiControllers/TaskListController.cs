using BLL.Contracts;
using Globals;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using WebApp.Mappers;

namespace WebApp.ApiControllers
{
    [Route("api/taskLists")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _service;

        public TaskListController(ITaskListService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all lists filtered if filters are specified
        /// </summary>
        /// <returns>Collection of task lists</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<TaskListDTO>), 200)]
        public async Task<ActionResult<IEnumerable<TaskListDTO>>> GetTaskLists(
            [FromQuery] FilterDTO? filter)
        {
            var taskLists = await _service.AllAsync(filter);
            return Ok(taskLists.Select(TaskListMapper.Map));
        }

        /// <summary>
        /// Gets a specific task list by id
        /// </summary>
        /// <param name="id">The id of the task list</param>
        /// <returns>The requested task list</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskListDTO), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<TaskListDTO>> GetTaskList(Guid id)
        {
            var list = await _service.FindAsync(id);

            if (list == null)
            {
                return NoContent();
            }

            return Ok(TaskListMapper.Map(list));
        }

        /// <summary>
        /// Updates a specific task list
        /// </summary>
        /// <param name="id">The id of the task list to update</param>
        /// <param name="list">The updated task list data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutTaskList(Guid id, TaskListDTO list)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existingEntity = await _service.FindAsync(id);
            if (existingEntity == null)
            {
                return NotFound();
            }

            if (id != list.Id)
            {
                return BadRequest();
            }

            var listBLLDto = TaskListMapper.Map(list);

            await _service.UpdateAsync(listBLLDto);

            return NoContent();
        }

        /// <summary>
        /// Creates a new task list
        /// </summary>
        /// <param name="entity">The task list to create</param>
        /// <returns>The created task list</returns>
        [ProducesResponseType(typeof(TaskListDTO), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<ActionResult> PostTaskList(TaskListDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var listBLLDto = TaskListMapper.Map(entity);


            await _service.AddAsync(listBLLDto);

            var response = TaskListMapper.Map(listBLLDto);


            return CreatedAtAction("GetTaskList",
                new { id = response.Id }, response);
        }

        /// <summary>
        /// Deletes a specific task list
        /// </summary>
        /// <param name="id">The id of the task list to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTaskList(Guid id)
        {
            var taskList = await _service.FindAsync(id);

            if (taskList == null)
            {
                return NoContent();
            }

            await _service.RemoveAsync(taskList.Id);

            return NoContent();
        }
    }
}