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
        return dalItems.Select(ListItemBLLMapper.Map)!;
    }

    public async Task<ListItemBLLDTO?> FindAsync(Guid id)
    {
        var dalItem = await repository.FindAsync(id);
        return ListItemBLLMapper.Map(dalItem);
    }

    public void Add(ListItemBLLDTO entity)
    {
        var dalEntity = ListItemBLLMapper.Map(entity);
        if (dalEntity != null)
        {
            repository.Add(dalEntity);
        }
    }

    public ListItemBLLDTO Update(ListItemBLLDTO entity)
    {
        var dalEntity = ListItemBLLMapper.Map(entity);
        var updatedDalEntity = repository.Update(dalEntity!);
        return ListItemBLLMapper.Map(updatedDalEntity)!;
    }

    public void Remove(Guid id) => repository.Remove(id);
    public async Task<IEnumerable<ListItemBLLDTO>> GetListItemsByTaskList(Guid taskListId)
    {
        var dalItems = await repository.GetListItemsByTaskList(taskListId);
        return dalItems.Select(ListItemBLLMapper.Map)!;
    }
}