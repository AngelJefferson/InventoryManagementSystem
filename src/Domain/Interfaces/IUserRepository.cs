using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task AddAsync(User user);
}
