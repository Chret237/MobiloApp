using Microsoft.Maui.Controls.Shapes;
using MobiloApp.Controls;
using MobiloApp.Models;

namespace MobiloApp.Pages;

public partial class SearchPage : MobiloPage
{
	public List<Vehicle> Vehicles => Sample.Vehicles;
	string _category = "All";

	public SearchPage()
	{
		InitializeComponent();
		BindingContext = this;
		BuildFilters();
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
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(999) }, StrokeThickness = 1,
				Stroke = active ? Theme.Forest : Theme.Line, BackgroundColor = active ? Theme.Forest : Colors.White,
				Padding = new Thickness(13, 8), Content = row
			};
			var tap = new TapGestureRecognizer();
			var captured = item;
			tap.Tapped += (_, _) => { _category = captured; BuildFilters(); };
			chip.GestureRecognizers.Add(tap);
			Filters.Children.Add(chip);
		}
	}

	async void OnBack(object? sender, EventArgs e) => await Nav.Go("home");
	async void OnViewCar(object? sender, Vehicle v) => await Nav.Go("details");
}
