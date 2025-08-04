using BLL.DTOs;
using DAL.Contracts;

namespace BLL.Contracts;

public interface IListItemService
{

    Task<ListItemBLLDTO?> FindAsync(Guid id);

    Task AddAsync(ListItemBLLDTO entity);

    Task<ListItemBLLDTO> UpdateAsync(ListItemBLLDTO entity);

    Task RemoveAsync(Guid id);
}