using BLL.DTOs;
using DAL.DTOs;

namespace BLL.Mappers;

public class TaskListBLLMapper
{
    public static TaskListDalDTO Map(TaskListBLLDTO dto)
    {
        return new TaskListDalDTO
        {
            Id = dto.Id,
            Title = dto.Title,
            ListItems = dto.ListItems?.Select(i => new ListItemDalDTO
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

    public static TaskListBLLDTO Map(TaskListDalDTO entity)
    {
        return new TaskListBLLDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            ListItems = entity.ListItems?.Select(i => new ListItemBLLDTO
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