using BLL.DTOs;
using DAL.Contracts;

namespace BLL.Contracts;

public interface IListItemService
{
    Task<IEnumerable<ListItemBLLDTO>> AllAsync();

    Task<ListItemBLLDTO?> FindAsync(Guid id);

    Task AddAsync(ListItemBLLDTO entity);

    Task<ListItemBLLDTO> UpdateAsync(ListItemBLLDTO entity);

    Task RemoveAsync(Guid id);
    Task<IEnumerable<ListItemBLLDTO>> GetListItemsByTaskList(Guid taskListId);
}