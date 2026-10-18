namespace MobiloApp.Controls;

/// <summary>Screen header: optional back button, subtitle + title, optional right icon button (AppHeader component).</summary>
public class AppHeader : ControlBase
{
	public static readonly BindableProperty TitleProperty = Prop<AppHeader, string>(nameof(Title), "");
	public static readonly BindableProperty SubtitleProperty = Prop<AppHeader, string>(nameof(Subtitle), "");
	public static readonly BindableProperty ShowBackProperty = Prop<AppHeader, bool>(nameof(ShowBack), false);
	public static readonly BindableProperty DarkProperty = Prop<AppHeader, bool>(nameof(Dark), false);
	public static readonly BindableProperty RightIconProperty = Prop<AppHeader, string>(nameof(RightIcon), "");

	public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
	public string Subtitle { get => (string)GetValue(SubtitleProperty); set => SetValue(SubtitleProperty, value); }
	public bool ShowBack { get => (bool)GetValue(ShowBackProperty); set => SetValue(ShowBackProperty, value); }
	/// <summary>True when drawn on the dark green hero (white text, glass buttons).</summary>
	public bool Dark { get => (bool)GetValue(DarkProperty); set => SetValue(DarkProperty, value); }
	public string RightIcon { get => (string)GetValue(RightIconProperty); set => SetValue(RightIconProperty, value); }

	public event EventHandler? BackClicked;
	public event EventHandler? RightClicked;

	public AppHeader() => Ready();

	protected override void Rebuild()
	{
		var grid = new Grid
		{
			Padding = new Thickness(22, 24, 22, 16), ColumnSpacing = 12,
			ColumnDefinitions = { new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto) }
		};

		if (ShowBack)
		{
			var back = new IconButton { Icon = "back", Glass = Dark };
			back.Clicked += (_, _) => BackClicked?.Invoke(this, EventArgs.Empty);
			grid.Add(back, 0);
		}

		var titles = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Spacing = 4 };
		if (!string.IsNullOrEmpty(Subtitle))
			titles.Add(new Label { Text = Subtitle, FontSize = 13, TextColor = Dark ? Colors.White.WithAlpha(.62f) : Theme.InkSoft });
		titles.Add(new Label
		{
			Text = Title, FontFamily = Theme.Display, FontSize = 25, CharacterSpacing = -1, LineHeight = 1.15,
			TextColor = Dark ? Colors.White : Theme.Ink
		});
		grid.Add(titles, 1);

		if (!string.IsNullOrEmpty(RightIcon))
		{
			var right = new IconButton { Icon = RightIcon, Glass = Dark };
			right.Clicked += (_, _) => RightClicked?.Invoke(this, EventArgs.Empty);
			grid.Add(right, 2);
		}
		else if (ShowBack)
			grid.Add(new BoxView { WidthRequest = 42, HeightRequest = 42, Color = Colors.Transparent }, 2);

		Content = grid;
	}
}
