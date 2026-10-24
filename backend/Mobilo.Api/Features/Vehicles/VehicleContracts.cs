using System.ComponentModel.DataAnnotations;
using Mobilo.Api.Domain;

namespace Mobilo.Api.Features.Vehicles;

/// <summary>Query parameters used to search the active vehicle catalogue.</summary>
public sealed record VehicleSearchRequest
{
    /// <summary>Free-text search over vehicle name and type.</summary>
    [MaxLength(100)]
    public string? Search { get; init; }

    /// <summary>Vehicle category, such as SUV or Luxury.</summary>
    [MaxLength(80)]
    public string? Category { get; init; }

    /// <summary>Maximum daily price in the requested currency.</summary>
    [Range(0, 100000)]
    public decimal? MaxPrice { get; init; }

    /// <summary>Page number starting at one.</summary>
    [Range(1, 10000)]
    public int? Page { get; init; }

    /// <summary>Number of records per page.</summary>
    [Range(1, 100)]
    public int? PageSize { get; init; }
}

/// <summary>Vehicle returned by the catalogue API.</summary>
public sealed record VehicleResponse(
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
    string Location);

/// <summary>Paginated vehicle catalogue response.</summary>
public sealed record VehicleListResponse(
    IReadOnlyList<VehicleResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);

public static class VehicleMappings
{
    public static VehicleResponse ToResponse(this Vehicle vehicle) => new(
        vehicle.Id,
        vehicle.Name,
        vehicle.Type,
        vehicle.Description,
        vehicle.ImageUrl,
        vehicle.PricePerDay,
        vehicle.Rating,
        vehicle.ReviewCount,
        vehicle.Seats,
        vehicle.Bags,
        vehicle.Gearbox,
        vehicle.Location);
}
