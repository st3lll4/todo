using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using DAL.DTOs;
using Globals;
using Moq;

namespace Tests;

public class TaskListTests
{
    [Fact]
    public async Task TaskListServiceGetAllAsyncFilter_ShouldFindFilteredTaskLists()
    {
        var mockRepo = new Mock<ITaskListRepository>();

        var list = new TaskListDalDTO
        {
            Id = Guid.NewGuid(),
            Title = "Birthday",
            CreatedAt = DateTime.UtcNow,
            ListItems = new List<ListItemDalDTO>
            {
                new ListItemDalDTO
                {
                    Id = Guid.NewGuid(),
                    Description = "Make the playlist",
                    IsDone = false,
                    Priority = EPriorityLevel.Medium,
                    CreatedAt = DateTime.UtcNow,
                },
                new ListItemDalDTO {
                    Id = Guid.NewGuid(),
                    Description = "Buy balloons",
                    IsDone = false,
                    Priority = EPriorityLevel.High,
                    CreatedAt = DateTime.UtcNow,
                }
            }
        };

        var filter = new FilterDTO
        {
            IncludesText = "Balloon" 
        };
        
        var allLists = new List<TaskListDalDTO> { list };
        
        mockRepo.Setup(r => r.AddAsync(It.IsAny<TaskListDalDTO>()))
            .Returns(Task.CompletedTask);
        
        mockRepo.Setup(r => r.AllAsync(It.IsAny<FilterDTO?>()))
            .ReturnsAsync((FilterDTO? f) =>
            {
                return allLists.Where(l => l.ListItems!.Any(item =>
                        item.Description.Contains(f!.IncludesText!, StringComparison.OrdinalIgnoreCase))
                );
            });
        
        var service = new TaskListService(mockRepo.Object);
        
        var listBLLDTO = TaskListBLLMapper.Map(list);
        await service.AddAsync(listBLLDTO);

        var filteredLists = await service.AllAsync(filter);
        
        Assert.NotEqual(filteredLists, []);
    }
    
    [Fact]
    public async Task TaskListServiceAddAsync_ShouldCreateTaskList()
    {
        var mockRepo = new Mock<ITaskListRepository>();

        var list = new TaskListDalDTO
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<TaskListDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => list?.Id == id ? list : null);

        var service = new TaskListService(mockRepo.Object);

        var listBLLDTO = TaskListBLLMapper.Map(list);

        await service.AddAsync(listBLLDTO);

        var listQueried = await service.FindAsync(list.Id);

        Assert.NotNull(listQueried);
        Assert.Equal(list.Id, listQueried.Id);
    }

    [Fact]
    public async Task TaskListServiceUpdateAsync_ShouldUpdateTaskList()
    {
        var mockRepo = new Mock<ITaskListRepository>();

        var list = new TaskListDalDTO
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<TaskListDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskListDalDTO>()))
            .ReturnsAsync((TaskListDalDTO updatedEntity) => updatedEntity);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => list?.Id == id ? list : null);

        var service = new TaskListService(mockRepo.Object);

        var listBLLDTO = TaskListBLLMapper.Map(list);

        await service.AddAsync(listBLLDTO);

        var listToUpdate = await service.FindAsync(list.Id);
        Assert.NotNull(listToUpdate);

        listToUpdate.Title = "Updated";
        await service.UpdateAsync(listToUpdate);

        var updatedList = await service.FindAsync(listToUpdate.Id);
        Assert.NotNull(updatedList);

        Assert.NotEqual(updatedList.Title, listToUpdate.Title);
    }


    [Fact]
    public async Task TaskListServiceRemoveAsync_ShouldDeleteTaskList()
    {
        var mockRepo = new Mock<ITaskListRepository>();

        var list = new TaskListDalDTO
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            CreatedAt = DateTime.UtcNow,
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<TaskListDalDTO>()))
            .Returns(Task.CompletedTask);

        mockRepo.Setup(r => r.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => list?.Id == id ? list : null);

        mockRepo.Setup(r => r.RemoveAsync(It.IsAny<Guid>()))
            .Callback((Guid id) => {
                if (list?.Id == id) list = null;
            })
            .Returns(Task.CompletedTask);

        var service = new TaskListService(mockRepo.Object);

        var listBLLDTO = TaskListBLLMapper.Map(list);

        await service.AddAsync(listBLLDTO);

        var listToRemove = await service.FindAsync(list.Id);
        Assert.NotNull(listToRemove);

        await service.RemoveAsync(listToRemove.Id);
        
        var removedList = await service.FindAsync(listToRemove.Id);
        Assert.Null(removedList);
    }
}