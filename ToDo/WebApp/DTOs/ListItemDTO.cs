using System.ComponentModel.DataAnnotations;
using Globals;

namespace WebApp.DTOs;

public class ListItemDTO
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Description field is required")]
    [MaxLength(512)]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Is done field is required")]
    public bool IsDone { get; set; }

    [Required(ErrorMessage = "Priority level is required")]
    public EPriorityLevel Priority { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DueAt { get; set; }

    public Guid TaskListId { get; set; }
    public TaskListDTO? TaskList { get; set; }

    public Guid? ParentItemId { get; set; }
    public ListItemDTO? ParentItem { get; set; }

    public ICollection<ListItemDTO>? SubItems { get; set; }
}