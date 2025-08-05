using DAL.DTOs;
using Domain;

namespace DAL.Mappers;

public class TaskListDalMapper 

{
    public static TaskList Map(TaskListDalDTO dto)
    {
        return new TaskList
        {
            Id = dto.Id,
            Title = dto.Title,
            ListItems = dto.ListItems?.Select(i => new ListItem
            {
                Id = i.Id,
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                DueAt = i.DueAt,
                TaskListId = i.TaskListId,
                ParentItemId = i.ParentItemId,
            }).ToList()
        };
    }

    public static TaskListDalDTO Map(TaskList entity)
    {
        return new TaskListDalDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            ListItems = entity.ListItems?.Select(i => new ListItemDalDTO
            {
                Id = i.Id,
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                CreatedAt = i.CreatedAt,
                DueAt = i.DueAt,
                TaskListId = i.TaskListId
            }).ToList()
        };
    }
}