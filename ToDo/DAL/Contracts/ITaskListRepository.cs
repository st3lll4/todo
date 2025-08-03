using DAL.DTOs;
using Globals;

namespace DAL.Contracts;

public interface ITaskListRepository
{
    Task<IEnumerable<TaskListDalDTO>> AllAsync(FilterDTO? filter);

    Task<TaskListDalDTO?> FindAsync(Guid id);

    Task AddAsync(TaskListDalDTO entity);
        
    Task<TaskListDalDTO> UpdateAsync(TaskListDalDTO entity);

    Task RemoveAsync(Guid id);
}