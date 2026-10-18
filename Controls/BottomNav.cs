namespace MobiloApp.Controls;

/// <summary>5-tab bottom navigation (BottomNav component). Set Active to the current route.</summary>
public class BottomNav : ControlBase
{
	public static readonly BindableProperty ActiveProperty = Prop<BottomNav, string>(nameof(Active), "home");
	public string Active { get => (string)GetValue(ActiveProperty); set => SetValue(ActiveProperty, value); }

	static readonly (string Label, string Icon, string Route)[] Items =
	{
		("Home", "home", "home"), ("Search", "search", "search"), ("Bookings", "calendar", "bookings"),
		("Favorites", "heart", "favorites"), ("Profile", "user", "profile"),
	};

	public BottomNav() => Ready();

	protected override void Rebuild()
	{
		var grid = new Grid { Padding = new Thickness(8, 7, 8, 10), HeightRequest = 78 };
		for (var i = 0; i < Items.Length; i++)
		{
			grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
			var (label, icon, route) = Items[i];
			var active = Active == route;
			var color = active ? Theme.Forest : Theme.Muted;

			var item = new VerticalStackLayout
			{
				Spacing = 5, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center,
				Children =
				{
					new IconView { Icon = icon, Color = color, StrokeWidth = active ? 2.4f : 1.8f },
					new Label { Text = label, FontSize = 9, TextColor = color, FontFamily = active ? Theme.BodyBold : Theme.Body, HorizontalTextAlignment = TextAlignment.Center }
				}
			};
			Tap(item, () => _ = Nav.Go(route));
			grid.Add(item, i);
		}

		Content = new VerticalStackLayout
		{
			Spacing = 0, BackgroundColor = Colors.White.WithAlpha(.95f),
			Children = { new BoxView { HeightRequest = 1, Color = Theme.Line }, grid }
		};
	}
}
