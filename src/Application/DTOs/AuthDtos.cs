namespace InventoryManagement.Application.DTOs;

public record LoginRequest(string Username, string Password);

public record RegisterRequest(string Username, string Password, string Email, string Role = "User");

public record AuthResponse(string Token, string Username, string Role);
