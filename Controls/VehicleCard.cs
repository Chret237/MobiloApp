using Microsoft.Maui.Controls.Shapes;
using MobiloApp.Models;

namespace MobiloApp.Controls;

/// <summary>Vehicle card: photo, heart, type badge, name, rating, price and "View car" (VehicleCard component).
/// Wide = full-width list variant used on the Search screen; otherwise a 300-wide carousel card.</summary>
public class VehicleCard : ControlBase
{
	public static readonly BindableProperty VehicleProperty = Prop<VehicleCard, Vehicle?>(nameof(Vehicle), null);
	public static readonly BindableProperty WideProperty = Prop<VehicleCard, bool>(nameof(Wide), false);

	public Vehicle? Vehicle { get => (Vehicle?)GetValue(VehicleProperty); set => SetValue(VehicleProperty, value); }
	public bool Wide { get => (bool)GetValue(WideProperty); set => SetValue(WideProperty, value); }
	public event EventHandler<Vehicle>? ViewClicked;

	public VehicleCard() => Ready();

	protected override void Rebuild()
	{
		if (Vehicle is not { } v) { Content = null; return; }

		var image = new Grid { HeightRequest = Wide ? 190 : 172 };
		image.Add(new Image { Source = v.Image, Aspect = Aspect.AspectFill });
		image.Add(new IconButton { Icon = "heart", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, Margin = new Thickness(0, 12, 12, 0) });
		image.Add(new Badge { Text = v.Type, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.End, Margin = new Thickness(12, 0, 0, 12) });

		var view = new HorizontalStackLayout { Spacing = 5, VerticalOptions = LayoutOptions.End };
		view.Add(new Label { Text = "View car", FontFamily = Theme.BodyBold, FontSize = 11, TextColor = Theme.Forest, VerticalOptions = LayoutOptions.Center });
		view.Add(new IconView { Icon = "arrow", IconSize = 16, Color = Theme.Forest });
		Tap(view, () => ViewClicked?.Invoke(this, v));

		var price = new HorizontalStackLayout { Spacing = 4, VerticalOptions = LayoutOptions.End };
		price.Add(new Label { Text = $"${v.Price}", FontFamily = Theme.Display, FontSize = 18, VerticalOptions = LayoutOptions.End });
		price.Add(new Label { Text = "/ day", FontSize = 11, TextColor = Theme.InkSoft, VerticalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 0, 2) });

		var footer = new Grid { Margin = new Thickness(0, 16, 0, 0), ColumnDefinitions = { new(GridLength.Star), new(GridLength.Auto) } };
		footer.Add(price, 0);
		footer.Add(view, 1);

		var body = new VerticalStackLayout { Padding = 15, Spacing = 5 };
		body.Add(new Label { Text = v.Name, FontFamily = Theme.Display, FontSize = 17, CharacterSpacing = -.4 });
		body.Add(new RatingView { Value = v.Rating, Count = v.Reviews });
		body.Add(footer);

		if (!Wide) WidthRequest = 300;
		Content = new Border
		{
			StrokeShape = Round(Theme.Radius), StrokeThickness = 1, Stroke = Theme.Line, BackgroundColor = Theme.Paper,
			Content = new VerticalStackLayout { Spacing = 0, Children = { image, body } }
		};
	}
}
