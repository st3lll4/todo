using BLL.Contracts;
using BLL.DTOs;
using BLL.Mappers;
using DAL.Contracts;

namespace BLL.Services;

public class TaskListService(ITaskListRepository repository) : ITaskListService
{
    public async Task<IEnumerable<TaskListBLLDTO>> AllAsync()
    {
        var dalItems = await repository.AllAsync();
        return dalItems.Select(TaskListBLLMapper.Map).Where(x => x != null)!;
    }

    public async Task<TaskListBLLDTO?> FindAsync(Guid id)
    {
        var dalItem = await repository.FindAsync(id);
        return TaskListBLLMapper.Map(dalItem);
    }

    public void Add(TaskListBLLDTO entity)
    {
        var dalEntity = TaskListBLLMapper.Map(entity);
        if (dalEntity != null)
        {
            repository.Add(dalEntity);
        }
    }

    public TaskListBLLDTO Update(TaskListBLLDTO entity)
    {
        var dalEntity = TaskListBLLMapper.Map(entity);
        var updatedDalEntity = repository.Update(dalEntity!);
        return TaskListBLLMapper.Map(updatedDalEntity)!;
    }

    public void Remove(Guid id)
    { 
        repository.Remove(id);
    }
    
}