using System.ComponentModel.DataAnnotations;

namespace Globals;

public class FilterDTO : IValidatableObject
{
    public DateTime? DueAtFrom { get; set; }    
    public DateTime? DueAtTo { get; set; }    
    public bool? Done { get; set; }   
    
    [MaxLength(256)]
    public string? IncludesText { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DueAtFrom > DueAtTo)
        {
            yield return new ValidationResult("The first date range has to be before second one", [nameof(DueAtTo), nameof(DueAtFrom)]);
        }
    }
}