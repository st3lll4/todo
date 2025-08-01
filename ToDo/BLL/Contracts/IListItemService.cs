using BLL.DTOs;
using DAL.Contracts;

namespace BLL.Contracts;

public interface IListItemService
{
    Task<IEnumerable<ListItemBLLDTO>> AllAsync();

    Task<ListItemBLLDTO?> FindAsync(Guid id);

    void Add(ListItemBLLDTO entity);

    ListItemBLLDTO Update(ListItemBLLDTO entity);

    void Remove(Guid id);
    Task<IEnumerable<ListItemBLLDTO>> GetListItemsByTaskList(Guid taskListId);
}