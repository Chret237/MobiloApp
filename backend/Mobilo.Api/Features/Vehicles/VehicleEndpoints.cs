using Microsoft.EntityFrameworkCore;
using Mobilo.Api.Data;

namespace Mobilo.Api.Features.Vehicles;

public static class VehicleEndpoints
{
    public static IEndpointRouteBuilder MapVehicleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/vehicles")
            .WithTags("Vehicles");

        group.MapGet("", async Task<VehicleListResponse> (
            [AsParameters] VehicleSearchRequest request,
            MobiloDbContext db,
            CancellationToken cancellationToken) =>
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            var query = db.Vehicles
                .AsNoTracking()
                .Where(vehicle => vehicle.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(vehicle =>
                    vehicle.Name.Contains(search) || vehicle.Type.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                var category = request.Category.Trim();
                query = query.Where(vehicle => vehicle.Type.Contains(category));
            }

            if (request.MaxPrice is not null)
            {
                query = query.Where(vehicle => vehicle.PricePerDay <= request.MaxPrice.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var vehicles = await query
                .OrderByDescending(vehicle => vehicle.Rating)
                .ThenBy(vehicle => vehicle.PricePerDay)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vehicle => vehicle.ToResponse())
                .ToListAsync(cancellationToken);

            return new VehicleListResponse(vehicles, page, pageSize, totalCount);
        })
        .WithName("ListVehicles")
        .WithSummary("Search the active vehicle catalogue");

        group.MapGet("/{id:int}", async Task<IResult> (
            int id,
            MobiloDbContext db,
            CancellationToken cancellationToken) =>
        {
            var vehicle = await db.Vehicles
                .AsNoTracking()
                .Where(item => item.Id == id && item.IsActive)
                .Select(item => item.ToResponse())
                .SingleOrDefaultAsync(cancellationToken);

            return vehicle is null ? Results.NotFound() : Results.Ok(vehicle);
        })
        .WithName("GetVehicle")
        .WithSummary("Get one active vehicle by id");

        return endpoints;
    }
}
