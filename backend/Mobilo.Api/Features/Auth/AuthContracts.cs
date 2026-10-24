using System.ComponentModel.DataAnnotations;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Features.Auth;

/// <summary>Payload used to create a Mobilo account.</summary>
public sealed record RegisterRequest
{
    [Required, MinLength(2), MaxLength(120)]
    public required string FullName { get; init; }

    [Required, EmailAddress, MaxLength(200)]
    public required string Email { get; init; }

    [MaxLength(30)]
    public string? Phone { get; init; }

    [Required, MinLength(8), MaxLength(128)]
    public required string Password { get; init; }
}

/// <summary>Payload used to authenticate an existing account.</summary>
public sealed record LoginRequest
{
    [Required, EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}

/// <summary>Authenticated user returned by the API.</summary>
public sealed record UserResponse(int Id, string FullName, string Email, string? Phone);

/// <summary>Authentication result containing a bearer token and user profile.</summary>
public sealed record AuthResponse(string AccessToken, UserResponse User);

public static class UserMappings
{
    public static UserResponse ToResponse(this User user) =>
        new(user.Id, user.FullName, user.Email, user.Phone);
}
