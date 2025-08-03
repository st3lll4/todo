using BLL.DTOs;
using DAL.DTOs;

namespace BLL.Mappers;

public class ListItemBLLMapper 
{
    public static ListItemDalDTO Map(ListItemBLLDTO dto)
    {
        return new ListItemDalDTO
        {
            Id = dto.Id,
            Description = dto.Description,
            IsDone = dto.IsDone,
            Priority = dto.Priority,
            CreatedAt = dto.CreatedAt.ToUniversalTime(),
            DueAt = dto.DueAt?.ToUniversalTime(),
            TaskListId = dto.TaskListId,
            ParentItemId = dto.ParentItemId,

            SubItems = dto.SubItems?.Select(i => new ListItemDalDTO
            {
                Id = i.Id,
                Description = i.Description,
                IsDone = i.IsDone,
                Priority = i.Priority,
                CreatedAt = i.CreatedAt,
                DueAt = i.DueAt,
                TaskListId = i.TaskListId,
                ParentItemId = i.ParentItemId,
            }).ToList()
        };
    }

    public static ListItemBLLDTO Map(ListItemDalDTO entity)
    {
        return new ListItemBLLDTO
        {
            Id = entity.Id,
            Description = entity.Description,
            IsDone = entity.IsDone,
            Priority = entity.Priority,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            TaskListId = entity.TaskListId,
            ParentItemId = entity.ParentItemId,
            SubItems = entity.SubItems?.Select(i => new ListItemBLLDTO
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