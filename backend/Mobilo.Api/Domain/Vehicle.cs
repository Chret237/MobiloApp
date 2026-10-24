namespace Mobilo.Api.Domain;

public sealed class Vehicle
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal Rating { get; set; }
    public int ReviewCount { get; set; }
    public int Seats { get; set; }
    public int Bags { get; set; }
    public required string Gearbox { get; set; }
    public required string Location { get; set; }
    public bool IsActive { get; set; }
}
