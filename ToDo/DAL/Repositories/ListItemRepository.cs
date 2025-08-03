using DAL.Contracts;
using DAL.DTOs;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ListItemRepository(AppDbContext dbContext) : IListItemRepository
{
    public async Task<IEnumerable<ListItemDalDTO>> AllAsync()
    {
        return await dbContext.ListItems
            .AsNoTracking()
            .OrderBy(e => e.CreatedAt)
            .Select(e => ListItemDalMapper.Map(e))
            .ToListAsync();
    }

    public async Task<ListItemDalDTO?> FindAsync(Guid id)
    {
        return await dbContext.ListItems
            .AsNoTracking()
            .Where(e => e.Id.Equals(id))
            .Select(e => ListItemDalMapper.Map(e))
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(ListItemDalDTO entity)
    {
        var domainEntity = ListItemDalMapper.Map(entity);
        dbContext.Add(domainEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<ListItemDalDTO> UpdateAsync(ListItemDalDTO entity)
    {
        var domainEntity = ListItemDalMapper.Map(entity);
        dbContext.Update(domainEntity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task RemoveAsync(Guid id)
    {
        var dbEntity = await dbContext.ListItems.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (dbEntity == null) return;
        dbContext.Remove(dbEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ListItemDalDTO>> GetListItemsByTaskList(Guid taskListId)
    {
        var items = await dbContext.ListItems
            .AsNoTracking()
            .OrderBy(e => e.CreatedAt)
            .Where(e => e.TaskListId.Equals(taskListId))
            .ToListAsync();
        return items.Select(ListItemDalMapper.Map);
    }
}