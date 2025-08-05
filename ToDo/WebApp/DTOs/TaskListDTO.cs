using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs;

public class TaskListDTO
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Title field is required")]
    [MaxLength(128)]
    public string Title { get; set; } = default!;

    public DateTime? CreatedAt { get; set; }

    public ICollection<ListItemDTO>? ListItems { get; set; }
}