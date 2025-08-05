using BLL.DTOs;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using DAL.DTOs;
using Globals;
using Moq;

namespace Tests;

public class ListItemTests
{
    [Fact]
    public async Task ListItemServiceAddAsync_ShouldCreateListItem()
    {
        var mockRepo = new Mock<IListItemRepository>();

        var item = new ListItemDalDTO
        {
            Id = Guid.NewGuid(),
            Description = "Test item",
            IsDone = false,
            Priority = EPriorityLevel.Medium,
            DueAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<ListItemDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => item?.Id == id ? item : null);

        var service = new ListItemService(mockRepo.Object);

        var itemBLLDTO = ListItemBLLMapper.Map(item);

        await service.AddAsync(itemBLLDTO);

        var itemQueried = await service.FindAsync(item.Id);

        Assert.NotNull(itemQueried);
        Assert.Equal(item.Id, itemQueried.Id);
    }

    [Fact]
    public async Task ListItemServiceUpdateAsync_ShouldUpdateListItem()
    {
        var mockRepo = new Mock<IListItemRepository>();

        var item = new ListItemDalDTO
        {
            Id = Guid.NewGuid(),
            Description = "Test item",
            IsDone = false,
            Priority = EPriorityLevel.Medium,
            DueAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<ListItemDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ListItemDalDTO>()))
            .ReturnsAsync((ListItemDalDTO updatedEntity) => updatedEntity);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => item?.Id == id ? item : null);

        var service = new ListItemService(mockRepo.Object);

        var itemBLLDTO = ListItemBLLMapper.Map(item);

        await service.AddAsync(itemBLLDTO);

        var itemToUpdate = await service.FindAsync(itemBLLDTO.Id);
        Assert.NotNull(itemToUpdate);

        itemToUpdate.Description = "Updated";
        await service.UpdateAsync(itemToUpdate);

        var updatedList = await service.FindAsync(itemToUpdate.Id);
        Assert.NotNull(updatedList);

        Assert.NotEqual(updatedList.Description, itemToUpdate.Description);
    }


    [Fact]
    public async Task ListItemServiceRemoveAsync_ShouldRemoveListItem()
    {
        var mockRepo = new Mock<IListItemRepository>();

        var item = new ListItemDalDTO
        {
            Id = Guid.NewGuid(),
            Description = "Test item",
            IsDone = false,
            Priority = EPriorityLevel.Medium,
            DueAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<ListItemDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => item?.Id == id ? item : null);

        mockRepo.Setup(r => r.RemoveAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                if (item?.Id == id) item = null;
            })
            .Returns(Task.CompletedTask);

        var service = new ListItemService(mockRepo.Object);

        var itemBLLDTO = ListItemBLLMapper.Map(item);

        await service.AddAsync(itemBLLDTO);

        var itemToRemove = await service.FindAsync(item.Id);
        Assert.NotNull(itemToRemove);

        await service.RemoveAsync(itemToRemove.Id);

        var removedItem = await service.FindAsync(itemToRemove.Id);
        Assert.Null(removedItem);
    }

    [Fact]
    public async Task AddSubItems_ShouldAddNestedSubItemsInfinitely()
    {
        var mockRepo = new Mock<IListItemRepository>();

        var rootItem = new ListItemDalDTO
        {
            Id = Guid.NewGuid(),
            Description = "Root item",
            IsDone = false,
            Priority = EPriorityLevel.Medium,
            DueAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        var itemsById = new Dictionary<Guid, ListItemDalDTO> { [rootItem.Id] = rootItem };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<ListItemDalDTO>()))
            .Returns(Task.CompletedTask)
            .Callback<ListItemDalDTO>(item => itemsById[item.Id] = item); 

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => itemsById.GetValueOrDefault(id));
        
        var service = new ListItemService(mockRepo.Object);

        var rootBLLDTO = ListItemBLLMapper.Map(rootItem);
        await service.AddAsync(rootBLLDTO);

        var currentParent = rootBLLDTO;

        for (var i = 1; i <= 20; i++) // increase number but risk IDE crashing
        {
            var childDal = new ListItemDalDTO
            {
                Id = Guid.NewGuid(),
                Description = $"Child item level {i}",
                IsDone = false,
                Priority = EPriorityLevel.Low,
                CreatedAt = DateTime.UtcNow,
                ParentItemId = currentParent.Id
            };

            var childBLL = ListItemBLLMapper.Map(childDal);
            Assert.NotNull(childBLL);

            await service.AddAsync(childBLL);

            currentParent = childBLL; 
        }

        var lastChild = await service.FindAsync(currentParent.Id);
        Assert.NotNull(lastChild);
    }
}