using BLL.DTOs;
using WebApp.DTOs;

namespace WebApp.Mappers;

public class ListItemMapper
{
    public static ListItemBLLDTO Map(ListItemDTO dto)
    {
        return new ListItemBLLDTO
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Description = dto.Description,
            IsDone = dto.IsDone,
            Priority = dto.Priority,
            DueAt = dto.DueAt?.ToUniversalTime(),
            TaskListId = dto.TaskListId,
            ParentItemId = dto.ParentItemId,

            SubItems = dto.SubItems?.Select(i => new ListItemBLLDTO
            {
                Id = i.Id ?? Guid.NewGuid(),
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                DueAt = i.DueAt?.ToUniversalTime(),
                TaskListId = i.TaskListId,
                ParentItemId = i.ParentItemId,
            }).ToList()
        };
    }

    public static ListItemDTO Map(ListItemBLLDTO entity)
    {
        return new ListItemDTO
        {
            Id = entity.Id,
            Description = entity.Description,
            IsDone = entity.IsDone,
            Priority = entity.Priority,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            TaskListId = entity.TaskListId,
            ParentItemId = entity.ParentItemId,
            SubItems = entity.SubItems?.Select(i => new ListItemDTO
            {
                Id = i.Id,
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                CreatedAt = i.CreatedAt,
                DueAt = i.DueAt,
                TaskListId = i.TaskListId,
                ParentItemId = entity.ParentItemId,
            }).ToList()
        };
    }
}