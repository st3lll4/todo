using Globals;

namespace BLL.DTOs;

public class ListItemBLLDTO
{
    public Guid Id { get; set; }

    public string Description { get; set; } = default!;
    public bool IsDone { get; set; }
    public EPriorityLevel Priority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DueAt { get; set; }

    public Guid TaskListId { get; set; }
    public TaskListBLLDTO? TaskList { get; set; }


    public Guid? ParentItemId { get; set; }
    public ListItemBLLDTO? ParentItem { get; set; }

    public ICollection<ListItemBLLDTO>? SubItems { get; set; }
}