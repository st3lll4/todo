using DAL.Contracts;
using DAL.DTOs;
using DAL.Mappers;
using Globals;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class TaskListRepository(AppDbContext dbContext) : ITaskListRepository
{
    public async Task<IEnumerable<TaskListDalDTO>> AllAsync(FilterDTO? filter)
    {
        var query = dbContext.TaskLists
            .Include(tl => tl.ListItems!.OrderBy(i => i.CreatedAt).ThenBy(e => e.IsDone))
            .AsNoTracking();

        if (filter?.Done.HasValue == true)
        {
            query = query.Where(e => (e.ListItems != null) && e.ListItems.Any(i => i.IsDone == filter.Done.Value));
        }

        if (filter?.DueAtFrom.HasValue == true && filter?.DueAtTo.HasValue == true)
        {
            var fromUtc = filter.DueAtFrom.Value.ToUniversalTime();
            var toUtc = filter.DueAtTo.Value.ToUniversalTime();
            query = query.Where(e => e.ListItems != null &&
                                     e.ListItems.Any(i =>
                                         i.DueAt.HasValue && i.DueAt.Value >= fromUtc && i.DueAt.Value <= toUtc));
        }


        if (filter?.IncludesText != null)
        {
            query = query.Where(e => e.Title.Contains(filter.IncludesText)
                                     || e.ListItems != null
                                     && e.ListItems.Any(i => i.Description.Contains(filter.IncludesText)));
        }

        if (filter?.Priority.HasValue == true)
        {
            query = query.Where(e => e.ListItems != null && e.ListItems.Any(i => i.Priority == filter.Priority.Value));
        }

        return await query
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => TaskListDalMapper.Map(e))
            .ToListAsync();
    }

    public async Task<TaskListDalDTO?> FindAsync(Guid id)
    {
        return await dbContext.TaskLists
            .Include(tl => tl.ListItems!.OrderBy(i => i.CreatedAt).ThenBy(e => e.IsDone))
            .AsNoTracking()
            .Where(e => e.Id.Equals(id))
            .Select(e => TaskListDalMapper.Map(e))
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        dbContext.Add(domainEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TaskListDalDTO> UpdateAsync(TaskListDalDTO entity)
    {
        var domainEntity = TaskListDalMapper.Map(entity);
        dbContext.Update(domainEntity);
        await dbContext.SaveChangesAsync();
        return entity;
    }


    public async Task RemoveAsync(Guid id)
    {
        var dbEntity = await dbContext.TaskLists.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (dbEntity == null) return;

        dbContext.Remove(dbEntity);
        await dbContext.SaveChangesAsync();
    }
}