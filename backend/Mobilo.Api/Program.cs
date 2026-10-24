using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Mobilo.Api.Data;
using Mobilo.Api.Features.Auth;
using Mobilo.Api.Features.Vehicles;

var builder = WebApplication.CreateBuilder(args);
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

builder.Services.AddDbContext<MobiloDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Mobilo") ?? "Data Source=mobilo.db"));
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MobiloDbContext>();
    await SeedData.InitializeAsync(db, CancellationToken.None);
}

app.MapGet("/", () => Results.Ok(new { service = "Mobilo API", status = "ok" }));
app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapVehicleEndpoints();

app.Run();

public partial class Program;
