using DAL.Contracts;
using DAL.DTOs;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class TaskListRepository(AppDbContext dbContext) : ITaskListRepository
{
    public async Task<IEnumerable<TaskListDalDTO>> AllAsync()
    {
        return await dbContext.TaskLists.Select(e => TaskListDalMapper.Map(e)!).ToListAsync();
    }

    public async Task<TaskListDalDTO?> FindAsync(Guid id)
    {
        var entity = await dbContext.TaskLists.FirstOrDefaultAsync(e => e.Id.Equals(id));
        return TaskListDalMapper.Map(entity);
    }

    public void Add(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        if (domainEntity == null)
        {
            throw new ArgumentException("Failed to map entity to domain object", nameof(entity));
        }
        dbContext.Add(domainEntity);
    }

    public TaskListDalDTO Update(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        if (domainEntity == null)
        {
            throw new ArgumentException("Failed to map entity to domain object", nameof(entity));
        }
        dbContext.Update(domainEntity);
        return entity;
    }

    public void Remove(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        if (domainEntity == null)
        {
            throw new ArgumentException("Failed to map entity to domain object", nameof(entity));
        }
        dbContext.Remove(domainEntity);
    }

    public async Task RemoveAsync(Guid id)
    {
        var dbEntity = await dbContext.TaskLists.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (dbEntity != null)
        {
            dbContext.Remove(dbEntity);
        }
    }
}