using BLL.DTOs;
using Globals;

namespace BLL.Contracts;

public interface ITaskListService
{
    Task<IEnumerable<TaskListBLLDTO>> AllAsync(FilterDTO? filter);

    Task<TaskListBLLDTO?> FindAsync(Guid id);

    Task AddAsync(TaskListBLLDTO entity);

    Task<TaskListBLLDTO> UpdateAsync(TaskListBLLDTO entity);

    Task RemoveAsync(Guid id);
}