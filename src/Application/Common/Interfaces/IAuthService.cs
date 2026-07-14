using InventoryManagement.Application.DTOs;

namespace InventoryManagement.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
}
