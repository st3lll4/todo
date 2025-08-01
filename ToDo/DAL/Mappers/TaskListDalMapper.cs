using DAL.DTOs;
using Domain;
using Globals;

namespace DAL.Mappers;

public class TaskListDalMapper 

{
    public static TaskList? Map(TaskListDalDTO? dto)
    {
        if (dto == null) return null;

        return new TaskList
        {
            Id = dto.Id,
            Title = dto.Title,
            CreatedAt = dto.CreatedAt,
            ListItems = dto.ListItems?.Select(i => new ListItem
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

    public static TaskListDalDTO? Map(TaskList? entity)
    {
        if (entity == null) return null;
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