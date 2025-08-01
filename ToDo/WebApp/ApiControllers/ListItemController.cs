using BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using System.Linq;
using BLL.DTOs;
using WebApp.Mappers;

namespace WebApp.ApiControllers
{
    [Route("api/listItems")]
    [ApiController]
    public class ListItemController : ControllerBase
    {
        private readonly IListItemService _service;

        public ListItemController(IListItemService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ListItemDTO>), 200)]
        public async Task<ActionResult<IEnumerable<ListItemDTO>>> GetListItems()
        {
            var taskLists = await _service.AllAsync();
            return Ok(taskLists.Select(ListItemMapper.Map));
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ListItemDTO), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ListItemDTO>> GetListItem(Guid id)
        {
            var listItem = await _service.FindAsync(id);

            if (listItem == null)
            {
                return NoContent();
            }

            return Ok(ListItemMapper.Map(listItem));
        }
        
        [HttpGet("byTask/{taskListId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ListItemDTO>> GetListItemsByTaskList(Guid taskListId)
        {
            if (taskListId == Guid.Empty)
            {
                return BadRequest();
            }
            
            var result = await _service.GetListItemsByTaskList(taskListId);
            return Ok(result.Select(ListItemMapper.Map));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutListItem(Guid id, ListItemDTO listItem)
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

            if (id != listItem.Id)
            {
                return BadRequest();
            }

            var itemBLLDTO = ListItemMapper.Map(listItem);

            if (itemBLLDTO != null) _service.Update(itemBLLDTO);

            return NoContent();
        }

        [ProducesResponseType(typeof(TaskListDTO), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public ActionResult<ListItemDTO> PostListItem(ListItemDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var itemBLLDTO = ListItemMapper.Map(entity);

            if (itemBLLDTO == null)
            {
                return BadRequest();
            }

            _service.Add(itemBLLDTO);

            var response = ListItemMapper.Map(itemBLLDTO);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetListItem",
                new { id = response.Id }, response);
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteListItem(Guid id)
        {
            var item = await _service.FindAsync(id);

            if (item == null)
            {
                return NoContent();
            }

            _service.Remove(item.Id);

            return NoContent();
        }
    }
}
