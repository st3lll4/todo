using DAL.DTOs;

namespace DAL.Contracts;

public interface ITaskListRepository
{
    Task<IEnumerable<TaskListDalDTO>> AllAsync();

    Task<TaskListDalDTO?> FindAsync(Guid id);

    void Add(TaskListDalDTO entity);
        
    TaskListDalDTO Update(TaskListDalDTO entity);

    void Remove(Guid id);
}