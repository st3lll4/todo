using Globals;

namespace DAL.DataSeeding;

public static class InitialData
{
    public static readonly (string title, Guid? id)[]
        TaskLists =
        [
            ("Monday", null),
            ("Busy day", null),
            ("Party prep", null),
        ];

    public static readonly (string Description, bool IsDone, EPriorityLevel Priority, DateTime? DueAt, Guid? TaskId,
        string TaskTitle)[]
        Items =
        [
            ("Do laundry", false, EPriorityLevel.Low, new DateTime(2025, 8, 4), null, "Monday"),
            ("Clean house", false, EPriorityLevel.Medium, new DateTime(2025, 8, 4), null, "Monday"),
            ("Meal-prep", false, EPriorityLevel.Medium, new DateTime(2025, 8, 4), null, "Monday"),
            ("Go shopping", false, EPriorityLevel.Low, new DateTime(2025, 8, 4), null, "Monday"),
            ("Call mom", false, EPriorityLevel.Medium, null, null, "Busy day"),
            ("Team meeting", false, EPriorityLevel.High, null, null, "Busy day"),
            ("Doctor appointment", false, EPriorityLevel.Medium, null, null, "Busy day"),
            ("Wash the puppies", false, EPriorityLevel.Medium, null, null, "Busy day"),
            ("Work on KardPop", false, EPriorityLevel.Medium, null, null, "Busy day"),
            ("Buy decorations", false, EPriorityLevel.High, new DateTime(2025, 8, 19), null, "Party prep"),
            ("Send invitations", false, EPriorityLevel.High, new DateTime(2025, 8, 19), null, "Party prep"),
            ("Order food", false, EPriorityLevel.Medium, new DateTime(2025, 8, 19), null, "Party prep"),
            ("Make playlist", false, EPriorityLevel.Low, new DateTime(2025, 8, 19), null, "Party prep"),
            ("Hang dinosaur balloons", false, EPriorityLevel.High, new DateTime(2025, 8, 19), null, "Party prep")
        ];
}