using BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using WebApp.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for managing list items
    /// </summary>
    [Route("api/listItems")]
    [ApiController]
    public class ListItemController : ControllerBase
    {
        private readonly IListItemService _service;

        /// <summary>
        /// Constructor for ListItemController
        /// </summary>
        /// <param name="service">The list item service</param>
        public ListItemController(IListItemService service)
        {
            _service = service;
        }


        /// <summary>
        /// Gets a specific list item by id
        /// </summary>
        /// <param name="id">The id of the list item</param>
        /// <returns>The requested list item</returns>
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

        /// <summary>
        /// Updates a specific list item
        /// </summary>
        /// <param name="id">The id of the list item to update</param>
        /// <param name="listItem">The updated list item data</param>
        /// <returns>No content if successful</returns>
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

            await _service.UpdateAsync(itemBLLDTO);

            return NoContent();
        }

        /// <summary>
        /// Creates a new list item
        /// </summary>
        /// <param name="entity">The list item to create</param>
        /// <returns>The created list item</returns>
        [ProducesResponseType(typeof(TaskListDTO), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<ActionResult<ListItemDTO>> PostListItem(ListItemDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var itemBLLDTO = ListItemMapper.Map(entity);


            await _service.AddAsync(itemBLLDTO);

            var response = ListItemMapper.Map(itemBLLDTO);


            return CreatedAtAction("GetListItem",
                new { id = response.Id }, response);
        }

        /// <summary>
        /// Deletes a specific list item
        /// </summary>
        /// <param name="id">The id of the list item to delete</param>
        /// <returns>No content if successful</returns>
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

            await _service.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}