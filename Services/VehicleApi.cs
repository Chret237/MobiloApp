using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using MobiloApp.Models;

namespace MobiloApp.Services;

public sealed class VehicleApi(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Vehicle>> GetVehiclesAsync(
        string? search = null,
        string? category = null,
        decimal? maxPrice = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(search))
            query.Add($"search={Uri.EscapeDataString(search.Trim())}");
        if (!string.IsNullOrWhiteSpace(category))
            query.Add($"category={Uri.EscapeDataString(category.Trim())}");
        if (maxPrice is not null)
            query.Add($"maxPrice={maxPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        query.Add("page=1");
        query.Add("pageSize=100");

        var suffix = string.Join('&', query);
        try
        {
            var response = await httpClient.GetFromJsonAsync<VehicleListResponse>(
                $"api/vehicles?{suffix}", cancellationToken)
                ?? throw new InvalidDataException("The vehicle catalogue response was empty.");

            return response.Items.Select(item => item.ToVehicle()).ToList();
        }
        catch (Exception exception) when (exception is HttpRequestException or WebException or TaskCanceledException or JsonException or InvalidDataException)
        {
            throw new ApiUnavailableException("The Mobilo API is unavailable.", exception);
        }
    }

    public async Task<Vehicle?> GetVehicleAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/vehicles/{id}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            var vehicle = await response.Content.ReadFromJsonAsync<VehicleResponse>(cancellationToken);
            return vehicle?.ToVehicle();
        }
        catch (Exception exception) when (exception is HttpRequestException or WebException or TaskCanceledException or JsonException)
        {
            throw new ApiUnavailableException("The Mobilo API is unavailable.", exception);
        }
    }

    sealed record VehicleListResponse(IReadOnlyList<VehicleResponse> Items, int Page, int PageSize, int TotalCount);

    sealed record VehicleResponse(
        int Id,
        string Name,
        string Type,
        string Description,
        string ImageUrl,
        decimal PricePerDay,
        decimal Rating,
        int ReviewCount,
        int Seats,
        int Bags,
        string Gearbox,
        string Location)
    {
        public Vehicle ToVehicle() => new()
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Rating = Rating.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture),
            Reviews = ReviewCount,
            Price = PricePerDay,
            Image = ImageUrl,
            Seats = Seats,
            Bags = Bags,
            Gearbox = Gearbox,
            Location = Location,
        };
    }
}
