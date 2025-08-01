namespace DAL.DTOs;

public class TaskListDalDTO
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<ListItemDalDTO>? ListItems { get; set; }
}