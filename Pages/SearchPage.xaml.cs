using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using MobiloApp.Controls;
using MobiloApp.Models;
using MobiloApp.Services;

namespace MobiloApp.Pages;

public partial class SearchPage : MobiloPage
{
	public ObservableCollection<Vehicle> Vehicles { get; } = new();
	readonly VehicleApi _vehicleApi;
	string _category = "All";

	public SearchPage()
	{
		InitializeComponent();
		_vehicleApi = AppServices.Current.GetRequiredService<VehicleApi>();
		BindingContext = this;
		BuildFilters();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadVehiclesAsync();
	}

	async Task LoadVehiclesAsync()
	{
		try
		{
			var category = _category is "All" or "Under $100" ? null : _category;
			var maxPrice = _category == "Under $100" ? 100m : (decimal?)null;
			var vehicles = await _vehicleApi.GetVehiclesAsync(category: category, maxPrice: maxPrice);
			Vehicles.Clear();
			foreach (var vehicle in vehicles)
				Vehicles.Add(vehicle);
			VehicleCountText.Text = $"{Vehicles.Count} vehicles near Austin";
		}
		catch (ApiUnavailableException)
		{
			await Nav.Go("server-unavailable", new Dictionary<string, object>
			{
				["retryRoute"] = "search"
			});
		}
	}

	/// <summary>Filter chips: All (with filter icon), SUV, Luxury, Under $100.</summary>
	void BuildFilters()
	{
		Filters.Children.Clear();
		foreach (var item in new[] { "All", "SUV", "Luxury", "Under $100" })
		{
			var active = item == _category;
			var fg = active ? Colors.White : Theme.Ink;
			var row = new HorizontalStackLayout { Spacing = 5 };
			row.Add(new Label { Text = item, FontSize = 11, TextColor = fg, VerticalOptions = LayoutOptions.Center });
			if (item == "All") row.Add(new IconView { Icon = "filter", IconSize = 14, Color = fg });

			var chip = new Border
			{
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(999) },
				StrokeThickness = 1,
				Stroke = active ? Theme.Forest : Theme.Line,
				BackgroundColor = active ? Theme.Forest : Colors.White,
				Padding = new Thickness(13, 8),
				Content = row
			};
			var tap = new TapGestureRecognizer();
			var captured = item;
			tap.Tapped += async (_, _) => { _category = captured; BuildFilters(); await LoadVehiclesAsync(); };
			chip.GestureRecognizers.Add(tap);
			Filters.Children.Add(chip);
		}
	}

	async void OnBack(object? sender, EventArgs e) => await Nav.Go("home");
	async void OnViewCar(object? sender, Vehicle v) => await Nav.Go("details");
}
