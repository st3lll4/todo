using DAL.DTOs;
using Domain;
using Globals;

namespace DAL.Mappers;

public class ListItemDalMapper 
{
    public static ListItem Map(ListItemDalDTO dto)
    {
        return new ListItem
        {
            Id = dto.Id,
            Description = dto.Description,
            IsDone = dto.IsDone,
            Priority = dto.Priority,
            CreatedAt = dto.CreatedAt,
            DueAt = dto.DueAt,
            TaskListId = dto.TaskListId,
            ParentItemId = dto.ParentItemId,

            SubItems = dto.SubItems?.Select(i => new ListItem
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

    public static ListItemDalDTO Map(ListItem entity)
    {
        return new ListItemDalDTO
        {
            Id = entity.Id,
            Description = entity.Description,
            IsDone = entity.IsDone,
            Priority = entity.Priority,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            TaskListId = entity.TaskListId,
            ParentItemId = entity.ParentItemId,
            SubItems = entity.SubItems?.Select(i => new ListItemDalDTO
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