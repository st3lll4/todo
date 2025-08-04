using BLL.Contracts;
using BLL.DTOs;
using BLL.Mappers;
using DAL.Contracts;

namespace BLL.Services;

public class ListItemService(IListItemRepository repository) : IListItemService
{

    public async Task<ListItemBLLDTO?> FindAsync(Guid id)
    {
        var dalItem = await repository.FindAsync(id);
        if (dalItem == null) return null;
        return ListItemBLLMapper.Map(dalItem);
    }

    public async Task AddAsync(ListItemBLLDTO entity)
    {
        entity.CreatedAt ??= DateTime.UtcNow;
        var dalEntity = ListItemBLLMapper.Map(entity);
            await repository.AddAsync(dalEntity);
        
    }

    public async Task<ListItemBLLDTO> UpdateAsync(ListItemBLLDTO entity)
    {
        var dalEntity = ListItemBLLMapper.Map(entity);
        var updatedDalEntity = await repository.UpdateAsync(dalEntity);
        return ListItemBLLMapper.Map(updatedDalEntity);
    }

    public async Task RemoveAsync(Guid id) => await repository.RemoveAsync(id);
    
}