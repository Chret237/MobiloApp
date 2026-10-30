using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MobiloApp.Services;

public sealed class AuthService(HttpClient httpClient)
{
    const string TokenKey = "mobilo_access_token";

    public async Task SignInAsync(string email, string password, CancellationToken cancellationToken = default) =>
        await AuthenticateAsync("api/auth/login", new { email, password }, cancellationToken);

    public async Task SignUpAsync(string fullName, string email, string? phone, string password, CancellationToken cancellationToken = default) =>
        await AuthenticateAsync("api/auth/register", new { fullName, email, phone, password }, cancellationToken);

    public async Task<bool> RestoreSessionAsync()
    {
        var token = await SecureStorage.Default.GetAsync(TokenKey);
        if (string.IsNullOrWhiteSpace(token))
            return false;

        SetToken(token);
        return true;
    }

    public async Task SignOutAsync()
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
        SecureStorage.Default.Remove(TokenKey);
        await Task.CompletedTask;
    }

    async Task AuthenticateAsync(string endpoint, object payload, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(endpoint, payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiError>(cancellationToken);
                throw new InvalidOperationException(error?.Message ?? "Unable to authenticate with these details.");
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken)
                ?? throw new InvalidOperationException("The authentication response was empty.");
            SetToken(result.AccessToken);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception exception) when (exception is HttpRequestException or WebException or TaskCanceledException or JsonException)
        {
            throw new ApiUnavailableException("The Mobilo API is unavailable.", exception);
        }
    }

    void SetToken(string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        SecureStorage.Default.SetAsync(TokenKey, token);
    }

    sealed record AuthResponse(string AccessToken, UserResponse User);
    sealed record UserResponse(int Id, string FullName, string Email, string? Phone);
    sealed record ApiError(string? Message);
}
