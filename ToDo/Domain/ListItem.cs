using System.ComponentModel.DataAnnotations;
using Globals;

namespace Domain;

public class ListItem
{
    public Guid Id { get; set; }
    [MaxLength(512)] public string Description { get; set; } = default!;
    public bool IsDone { get; set; }
    public EPriorityLevel Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public Guid TaskListId { get; set; }    
    public TaskList? TaskList { get; set; }
    public Guid? ParentItemId { get; set; }
    public ListItem? ParentItem { get; set; }
    public ICollection<ListItem>? SubItems { get; set; }
}