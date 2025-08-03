using DAL.DTOs;

namespace DAL.Contracts;

public interface IListItemRepository
{
    Task<IEnumerable<ListItemDalDTO>> AllAsync();

    Task<ListItemDalDTO?> FindAsync(Guid id);

    Task AddAsync(ListItemDalDTO entity);

    Task<ListItemDalDTO> UpdateAsync(ListItemDalDTO entity);

    Task RemoveAsync(Guid id);
    Task<IEnumerable<ListItemDalDTO>> GetListItemsByTaskList(Guid taskListId);
}