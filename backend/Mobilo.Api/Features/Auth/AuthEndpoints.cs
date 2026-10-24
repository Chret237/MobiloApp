using Microsoft.EntityFrameworkCore;
using Mobilo.Api.Data;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async Task<IResult> (
            RegisterRequest request,
            MobiloDbContext db,
            PasswordHasher passwordHasher,
            TokenService tokenService,
            CancellationToken cancellationToken) =>
        {
            var email = request.Email.Trim().ToLowerInvariant();
            if (await db.Users.AnyAsync(user => user.Email == email, cancellationToken))
                return Results.Conflict(new { message = "An account with this email already exists." });

            var password = passwordHasher.Hash(request.Password);
            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = email,
                Phone = request.Phone?.Trim(),
                PasswordHash = password.Hash,
                PasswordSalt = password.Salt,
                CreatedAt = DateTimeOffset.UtcNow
            };

            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);
            return Results.Created("/api/me", new AuthResponse(tokenService.Create(user), user.ToResponse()));
        });

        group.MapPost("/login", async Task<IResult> (
            LoginRequest request,
            MobiloDbContext db,
            PasswordHasher passwordHasher,
            TokenService tokenService,
            CancellationToken cancellationToken) =>
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await db.Users.SingleOrDefaultAsync(item => item.Email == email, cancellationToken);
            if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
                return Results.Unauthorized();

            return Results.Ok(new AuthResponse(tokenService.Create(user), user.ToResponse()));
        });

        return endpoints;
    }

    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/me", async Task<IResult> (
            HttpContext context,
            MobiloDbContext db,
            TokenService tokenService,
            CancellationToken cancellationToken) =>
        {
            var userId = ReadUserId(context, tokenService);
            if (userId is null)
                return Results.Unauthorized();

            var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(item => item.Id == userId, cancellationToken);
            return user is null ? Results.NotFound() : Results.Ok(user.ToResponse());
        }).WithTags("Auth");

        return endpoints;
    }

    static int? ReadUserId(HttpContext context, TokenService tokenService)
    {
        var header = context.Request.Headers.Authorization.ToString();
        if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;
        return tokenService.Validate(header[7..].Trim());
    }
}
