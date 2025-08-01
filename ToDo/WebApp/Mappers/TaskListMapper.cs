using BLL.DTOs;
using WebApp.DTOs;

namespace WebApp.Mappers;

public class TaskListMapper
{
    public static TaskListBLLDTO? Map(TaskListDTO? dto)
    {
        if (dto == null) return null;

        return new TaskListBLLDTO
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Title = dto.Title,
            CreatedAt = dto.CreatedAt ?? DateTime.Now.ToUniversalTime(),
            ListItems = dto.ListItems?.Select(i => new ListItemBLLDTO
            {
                Id = i.Id ?? Guid.NewGuid(),
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                CreatedAt = i.CreatedAt ?? DateTime.Now.ToUniversalTime(),
                DueAt = i.DueAt?.ToUniversalTime(),
                TaskListId = i.TaskListId
            }).ToList()
        };
    }

    public static TaskListDTO? Map(TaskListBLLDTO? entity)
    {
        if (entity == null) return null;
        return new TaskListDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            ListItems = entity.ListItems?.Select(i => new ListItemDTO
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