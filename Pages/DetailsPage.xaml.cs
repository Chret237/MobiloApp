using MobiloApp.Controls;
using MobiloApp.Models;
using MobiloApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MobiloApp.Pages;

public partial class DetailsPage : MobiloPage, IQueryAttributable
{
	readonly VehicleApi _vehicleApi;
	int _vehicleId;
	Vehicle? _vehicle;

	public DetailsPage()
	{
		InitializeComponent();
		_vehicleApi = AppServices.Current.GetRequiredService<VehicleApi>();
		SizeChanged += (_, _) => Hero.HeightRequest = Math.Max(330, Height * 0.43); // 43dvh, min 330
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.TryGetValue("vehicleId", out var value) && int.TryParse(value?.ToString(), out var id))
			_vehicleId = id;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_vehicleId <= 0)
		{
			await Nav.Go("search");
			return;
		}

		try
		{
			_vehicle = await _vehicleApi.GetVehicleAsync(_vehicleId);
			if (_vehicle is null)
			{
				await Nav.Go("search");
				return;
			}

			VehicleNameLabel.Text = _vehicle.Name;
			VehicleSubtitleLabel.Text = $"{_vehicle.Type} · {_vehicle.Gearbox}";
			VehicleRating.Value = _vehicle.Rating;
			VehicleRating.Count = _vehicle.Reviews;
			SeatsLabel.Text = _vehicle.Seats.ToString();
			BagsLabel.Text = _vehicle.Bags.ToString();
			GearboxLabel.Text = _vehicle.Gearbox;
			LocationLabel.Text = _vehicle.Location;
			DayPriceLabel.Text = $"${_vehicle.Price:0}";
			HeroImage.Source = _vehicle.Image;
			BookingState.VehicleId = _vehicle.Id;
		}
		catch (ApiUnavailableException)
		{
			await Nav.Go("server-unavailable", new Dictionary<string, object>
			{
				["retryRoute"] = "details",
				["vehicleId"] = _vehicleId
			});
		}
	}

	async void OnBack(object? sender, EventArgs e) => await Nav.Go("search");
	async void OnBook(object? sender, EventArgs e) => await Nav.Go("booking");
}
