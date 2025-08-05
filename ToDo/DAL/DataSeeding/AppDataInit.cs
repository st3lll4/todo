using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataSeeding;

public static class AppDataInit
{
    public static void MigrateDatabase(AppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DeleteDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static async Task SeedData(AppDbContext context)
    {
        if (!context.TaskLists.Any())
        {
            var lists = InitialData.TaskLists.Select(t => new TaskList
            {
                Id = Guid.NewGuid(),
                Title = t.title
            });

            context.TaskLists.AddRange(lists);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($@"Error: {ex.Message}");
            Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
            Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
            throw;
        }

        if (!context.ListItems.Any())
        {
            var listDict = context.TaskLists.ToDictionary(l => l.Title, l => l);

            var itemsToAdd = new List<ListItem>();

            foreach (var itemData in InitialData.Items)
            {
                var item = new ListItem
                {
                    Id = Guid.NewGuid(),
                    Description = itemData.Description,
                    DueAt = itemData.DueAt?.ToUniversalTime(),
                    IsDone = itemData.IsDone,
                    Priority = itemData.Priority,
                };
                if (listDict.TryGetValue(itemData.TaskTitle, out var listEntity))
                {
                    item.TaskListId = listEntity.Id;
                    itemsToAdd.Add(item);
                }
                else
                {
                    Console.WriteLine($@"Missing list: {itemData.TaskTitle} for item: {itemData.Description}");
                }
            }

            context.ListItems.AddRange(itemsToAdd);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error: {ex.Message}");
                Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}