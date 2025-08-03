using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TaskList
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    // public int ItemCount { get; set; }  // TODO!
    // public int CompletedItemCount { get; set; }
    
    public ICollection<ListItem>? ListItems { get; set; }
}