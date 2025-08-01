namespace Domain;

public class TaskList
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<ListItem>? ListItems { get; set; }
    
}