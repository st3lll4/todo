using BLL.DTOs;

namespace BLL.Contracts;

public interface ITaskListService
{
    Task<IEnumerable<TaskListBLLDTO>> AllAsync();

    Task<TaskListBLLDTO?> FindAsync(Guid id);

    void Add(TaskListBLLDTO entity);

    TaskListBLLDTO Update(TaskListBLLDTO entity);

    void Remove(Guid id);
}