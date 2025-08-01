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
        dbContext.SaveChangesAsync(); 
    }

    public TaskListDalDTO Update(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        if (domainEntity == null)
        {
            throw new ArgumentException("Failed to map entity to domain object", nameof(entity));
        }
        dbContext.Update(domainEntity);
        dbContext.SaveChangesAsync(); 
        return entity;
    }
    

    public void Remove(Guid id)
    {
        var dbEntity = dbContext.TaskLists.FirstOrDefault(e => e.Id.Equals(id));
        if (dbEntity == null) return;
        dbContext.Remove(dbEntity);
        dbContext.SaveChangesAsync();
    }
}