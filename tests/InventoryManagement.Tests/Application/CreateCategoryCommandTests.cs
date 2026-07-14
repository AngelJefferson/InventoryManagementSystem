using InventoryManagement.Application.Categories.Commands;
using InventoryManagement.Application.Common.Interfaces;
using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Infrastructure.Data;

namespace InventoryManagement.Tests.Application;

public class CreateCategoryCommandTests
{
    private static IApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesCategory()
    {
        var context = CreateContext();
        var handler = new CreateCategoryCommandHandler(context);

        var result = await handler.Handle(new CreateCategoryCommand("Electronics", "Electronic items"), default);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Electronics", result.Name);
        Assert.Equal("Electronic items", result.Description);
    }

    [Fact]
    public async Task Handle_ValidCommand_PersistsToDatabase()
    {
        var context = CreateContext();
        var handler = new CreateCategoryCommandHandler(context);

        await handler.Handle(new CreateCategoryCommand("Books", "All kinds of books"), default);

        var saved = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Books");
        Assert.NotNull(saved);
        Assert.Equal("All kinds of books", saved.Description);
    }

    [Fact]
    public void Validate_EmptyName_Fails()
    {
        var validator = new CreateCategoryCommandValidator();

        var result = validator.Validate(new CreateCategoryCommand("", "desc"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_LongName_Fails()
    {
        var validator = new CreateCategoryCommandValidator();

        var result = validator.Validate(new CreateCategoryCommand(new string('a', 101), "desc"));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ValidCommand_Passes()
    {
        var validator = new CreateCategoryCommandValidator();

        var result = validator.Validate(new CreateCategoryCommand("Valid Name", "Valid description"));

        Assert.True(result.IsValid);
    }
}
