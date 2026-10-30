using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using MobiloApp.Controls;
using MobiloApp.Models;
using MobiloApp.Services;

namespace MobiloApp.Pages;

public partial class HomePage : MobiloPage
{
	public ObservableCollection<Vehicle> Vehicles { get; } = new();
	readonly VehicleApi _vehicleApi;

	public HomePage()
	{
		InitializeComponent();
		_vehicleApi = AppServices.Current.GetRequiredService<VehicleApi>();
		BackgroundColor = Theme.Cream;
		BindingContext = this;
		BuildCategories();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			var vehicles = await _vehicleApi.GetVehiclesAsync();
			Vehicles.Clear();
			foreach (var vehicle in vehicles.Take(2))
				Vehicles.Add(vehicle);
		}
		catch (ApiUnavailableException)
		{
			await Nav.Go("server-unavailable", new Dictionary<string, object>
			{
				["retryRoute"] = "home"
			});
		}
	}

	/// <summary>The 4 "Choose your ride" tiles (first one is the active one in the design).</summary>
	void BuildCategories()
	{
		var items = new[] { ("SUV", "car"), ("Sedan", "car"), ("Luxury", "star"), ("Pickup", "car") };
		for (var i = 0; i < items.Length; i++)
		{
			var (label, icon) = items[i];
			var active = i == 0;
			var tile = new Border
			{
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(Theme.RadiusSm) },
				StrokeThickness = 1,
				Stroke = active ? Colors.Transparent : Theme.Line,
				BackgroundColor = active ? Theme.Mint : Theme.Paper,
				Padding = new Thickness(4, 12),
				Content = new VerticalStackLayout
				{
					Spacing = 8,
					HorizontalOptions = LayoutOptions.Center,
					Children =
					{
						new Border
						{
							WidthRequest = 36, HeightRequest = 36, StrokeShape = new Ellipse(), StrokeThickness = 0,
							BackgroundColor = active ? Theme.Forest : Theme.Cream, HorizontalOptions = LayoutOptions.Center,
							Content = new IconView { Icon = icon, Color = active ? Colors.White : Theme.Ink }
						},
						new Label { Text = label, FontFamily = Theme.BodyBold, FontSize = 11, HorizontalTextAlignment = TextAlignment.Center,
							TextColor = active ? Theme.Forest : Theme.Ink }
					}
				}
			};
			var tap = new TapGestureRecognizer();
			tap.Tapped += async (_, _) => await Nav.Go("search");
			tile.GestureRecognizers.Add(tap);
			Categories.Add(tile, i);
		}
	}

	async void OnSearch(object? sender, EventArgs e) => await Nav.Go("search");
	async void OnSeeAll(object? sender, TappedEventArgs e) => await Nav.Go("search");
	async void OnViewCar(object? sender, Vehicle v) => await Nav.Go("details", new Dictionary<string, object>
	{
		["vehicleId"] = v.Id
	});

	async void OnBell(object? sender, EventArgs e)
	{
		Toast.IsVisible = true;
		await Task.Delay(2500);
		Toast.IsVisible = false;
	}

	void OnDismissToast(object? sender, EventArgs e) => Toast.IsVisible = false;
}
