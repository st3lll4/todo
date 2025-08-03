using System.ComponentModel.DataAnnotations;
using Globals;

namespace WebApp.DTOs;

public class ListItemDTO : IValidatableObject
{
    public Guid? Id { get; set; }
    
    [Required(ErrorMessage = "Description field is required")] [MaxLength(512)]
    public string Description { get; set; } = default!;
    public bool IsDone { get; set; }
    public EPriorityLevel Priority { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? DueAt { get; set; }
    
    public Guid TaskListId { get; set; }    
    public TaskListDTO? TaskList { get; set; }
    
    public Guid? ParentItemId { get; set; }
    public ListItemDTO? ParentItem { get; set; }

    public ICollection<ListItemDTO>? SubItems { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DueAt < DateTime.Now.ToUniversalTime())
        {
            yield return new ValidationResult("The task needs to be due at in the future", [nameof(DueAt)]);
        }
    }
}