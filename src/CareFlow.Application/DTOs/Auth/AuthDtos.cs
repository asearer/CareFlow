using CareFlow.Domain.Enums;

namespace CareFlow.Application.DTOs.Auth;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, Guid Id, string Email, string FirstName, string LastName, string Role);
