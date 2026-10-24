using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Features.Auth;

public sealed class TokenService(IConfiguration configuration)
{
    const string Header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
    readonly byte[] _secret = Encoding.UTF8.GetBytes(
        configuration["Auth:SigningKey"] ?? "mobilo-development-signing-key-change-me");

    public string Create(User user)
    {
        var payload = new
        {
            sub = user.Id.ToString(),
            email = user.Email,
            exp = DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds()
        };
        var encodedHeader = Base64Url(Encoding.UTF8.GetBytes(Header));
        var encodedPayload = Base64Url(JsonSerializer.SerializeToUtf8Bytes(payload));
        var content = $"{encodedHeader}.{encodedPayload}";
        var signature = Hmac(content);
        return $"{content}.{signature}";
    }

    public int? Validate(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3 || !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(parts[2]), Encoding.UTF8.GetBytes(Hmac($"{parts[0]}.{parts[1]}"))))
            return null;

        try
        {
            var payload = JsonSerializer.Deserialize<TokenPayload>(Base64UrlDecode(parts[1]));
            if (payload?.sub is null || payload.exp <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                return null;
            return int.Parse(payload.sub);
        }
        catch (Exception exception) when (exception is JsonException or FormatException or OverflowException)
        {
            return null;
        }
    }

    string Hmac(string content)
    {
        using var hmac = new HMACSHA256(_secret);
        return Base64Url(hmac.ComputeHash(Encoding.UTF8.GetBytes(content)));
    }

    static string Base64Url(byte[] bytes) => Convert.ToBase64String(bytes)
        .TrimEnd('=').Replace('+', '-').Replace('/', '_');

    static byte[] Base64UrlDecode(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }

    sealed record TokenPayload(string? sub, long exp);
}
