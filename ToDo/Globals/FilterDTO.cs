using System.ComponentModel.DataAnnotations;

namespace Globals;

public class FilterDTO : IValidatableObject
{
    public DateTime? DueAtFrom { get; set; }    
    public DateTime? DueAtTo { get; set; }    
    public bool? Done { get; set; }   
    
    [MaxLength(256)]
    public string? IncludesText { get; set; }
    
    public EPriorityLevel? Priority { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DueAtFrom > DueAtTo)
        {
            yield return new ValidationResult("The from date has to be before the to date", [nameof(DueAtFrom), nameof(DueAtTo)]);
        }
        
        if (DueAtFrom.HasValue && DueAtTo.HasValue == false || DueAtFrom.HasValue == false && DueAtTo.HasValue)
        {
            yield return new ValidationResult("Please specify both from and to dates for filtering", [nameof(DueAtFrom), nameof(DueAtTo)]);
        }
    }
}