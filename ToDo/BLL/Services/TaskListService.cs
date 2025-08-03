using BLL.Contracts;
using BLL.DTOs;
using BLL.Mappers;
using DAL.Contracts;
using Globals;

namespace BLL.Services;

public class TaskListService(ITaskListRepository repository) : ITaskListService
{
    public async Task<IEnumerable<TaskListBLLDTO>> AllAsync(FilterDTO? filter)
    {
        var dalItems = await repository.AllAsync(filter);
        return dalItems.Select(TaskListBLLMapper.Map);
    }

    public async Task<TaskListBLLDTO?> FindAsync(Guid id)
    {
        var dalItem = await repository.FindAsync(id);
        if (dalItem == null) return null;
        return TaskListBLLMapper.Map(dalItem);
    }

    public async Task AddAsync(TaskListBLLDTO entity)
    {
        var dalEntity = TaskListBLLMapper.Map(entity);
        await repository.AddAsync(dalEntity);
    }

    public async Task<TaskListBLLDTO> UpdateAsync(TaskListBLLDTO entity)
    {
        var dalEntity = TaskListBLLMapper.Map(entity);
        var updatedDalEntity = await repository.UpdateAsync(dalEntity);
        return TaskListBLLMapper.Map(updatedDalEntity);
    }

    public async Task RemoveAsync(Guid id)
    {
        await repository.RemoveAsync(id);
    }
}