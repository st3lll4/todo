namespace BLL.DTOs;

public class TaskListBLLDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public DateTime? CreatedAt { get; set; }

    public ICollection<ListItemBLLDTO>? ListItems { get; set; }
}