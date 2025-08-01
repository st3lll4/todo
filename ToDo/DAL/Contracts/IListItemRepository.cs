using DAL.DTOs;

namespace DAL.Contracts;

public interface IListItemRepository
{
    Task<IEnumerable<ListItemDalDTO>> AllAsync();

    Task<ListItemDalDTO?> FindAsync(Guid id);

    void Add(ListItemDalDTO entity);

    ListItemDalDTO Update(ListItemDalDTO entity);

    void Remove(Guid id);
    Task<IEnumerable<ListItemDalDTO>> GetListItemsByTaskList(Guid taskListId);
}