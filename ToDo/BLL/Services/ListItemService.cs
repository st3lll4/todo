using BLL.Contracts;
using BLL.DTOs;
using BLL.Mappers;
using DAL.Contracts;

namespace BLL.Services;

public class ListItemService(IListItemRepository repository) : IListItemService
{
    public async Task<IEnumerable<ListItemBLLDTO>> AllAsync()
    {
        var dalItems = await repository.AllAsync();
        return dalItems.Select(ListItemBLLMapper.Map);
    }

    public async Task<ListItemBLLDTO?> FindAsync(Guid id)
    {
        var dalItem = await repository.FindAsync(id);
        if (dalItem == null) return null;
        return ListItemBLLMapper.Map(dalItem);
    }

    public async Task AddAsync(ListItemBLLDTO entity)
    {
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
    
    public async Task<IEnumerable<ListItemBLLDTO>> GetListItemsByTaskList(Guid taskListId)
    {
        var dalItems = await repository.GetListItemsByTaskList(taskListId);
        return dalItems.Select(ListItemBLLMapper.Map);
    }
}