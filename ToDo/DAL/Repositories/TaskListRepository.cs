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

        var loweredText = filter?.IncludesText?.ToLower();

        var hasText = !string.IsNullOrWhiteSpace(loweredText);
        var hasPriority = filter?.Priority.HasValue == true;
        var hasDone = filter?.Done.HasValue == true;
        var hasDueRange = filter?.DueAtFrom.HasValue == true && filter?.DueAtTo.HasValue == true;

        if (!hasText && !hasPriority && !hasDone && !hasDueRange) {
            return await query
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => TaskListDalMapper.Map(e))
                .ToListAsync();
        }
        
        var fromUtc = filter?.DueAtFrom?.ToUniversalTime();
        var toUtc = filter?.DueAtTo?.ToUniversalTime();

        query = query.Where(e =>
            (hasText && e.Title.ToLower().Contains(loweredText!))
            ||
            (e.ListItems != null && e.ListItems.Any(i =>
                (!hasText || i.Description.ToLower().Contains(loweredText!)) &&
                (!hasPriority || i.Priority == filter.Priority) &&
                (!hasDone || i.IsDone == filter.Done) &&
                (!hasDueRange || (i.DueAt.HasValue && i.DueAt >= fromUtc && i.DueAt <= toUtc))
            ))
        );

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