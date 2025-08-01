using DAL.Contracts;
using DAL.DTOs;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ListItemRepository(AppDbContext dbContext) : IListItemRepository
{
    public async Task<IEnumerable<ListItemDalDTO>> AllAsync()
    {
        return await dbContext.ListItems.Select(e => ListItemDalMapper.Map(e)!).ToListAsync();
    }

    public async Task<ListItemDalDTO?> FindAsync(Guid id)
    {
        var entity = await dbContext.ListItems.FirstOrDefaultAsync(e => e.Id.Equals(id));
        return ListItemDalMapper.Map(entity);
    }

    public void Add(ListItemDalDTO entity)
    {
        var domainEntity = ListItemDalMapper.Map(entity);
        if (domainEntity == null)
        {
            throw new ArgumentException("Failed to map entity to domain object", nameof(entity));
        }

        dbContext.Add(domainEntity);
        dbContext.SaveChangesAsync();
    }

    public ListItemDalDTO Update(ListItemDalDTO entity)
    {
        var domainEntity = ListItemDalMapper.Map(entity);
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
        var dbEntity = dbContext.ListItems.FirstOrDefault(e => e.Id.Equals(id));
        if (dbEntity == null) return;
        dbContext.Remove(dbEntity);
        dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ListItemDalDTO>> GetListItemsByTaskList(Guid taskListId)
    {
        var items = await dbContext.ListItems.Where(e => e.TaskListId.Equals(taskListId)).ToListAsync();
        return items.Select(ListItemDalMapper.Map)!;
    }
}