using Globals;

namespace DAL.DTOs;

public class ListItemDalDTO
{
    
    public Guid Id { get; set; }
    
    public string Description { get; set; } = default!;
    public bool IsDone { get; set; }
    public EPriorityLevel Priority { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? DueAt { get; set; }
    
    public Guid TaskListId { get; set; }    
    public TaskListDalDTO? TaskList { get; set; } 

    
    public Guid? ParentItemId { get; set; }
    public ListItemDalDTO? ParentItem { get; set; }

    public ICollection<ListItemDalDTO>? SubItems { get; set; }

}