using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>Labelled text input with optional leading icon (Field component).</summary>
public class Field : ControlBase
{
	public static readonly BindableProperty LabelProperty = Prop<Field, string>(nameof(Label), "");
	public static readonly BindableProperty PlaceholderProperty = Prop<Field, string>(nameof(Placeholder), "");
	public static readonly BindableProperty IconProperty = Prop<Field, string>(nameof(Icon), "");
	public static readonly BindableProperty IsPasswordProperty = Prop<Field, bool>(nameof(IsPassword), false);
	public static readonly BindableProperty KeyboardTypeProperty = Prop<Field, string>(nameof(KeyboardType), "text");

	public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
	public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public bool IsPassword { get => (bool)GetValue(IsPasswordProperty); set => SetValue(IsPasswordProperty, value); }
	/// <summary>text | email | phone | numeric</summary>
	public string KeyboardType { get => (string)GetValue(KeyboardTypeProperty); set => SetValue(KeyboardTypeProperty, value); }

	public string Text => _entry?.Text ?? "";
	Entry? _entry;

	public Field() => Ready();

	protected override void Rebuild()
	{
		var entry = _entry = new Entry
		{
			Placeholder = Placeholder, PlaceholderColor = Theme.Placeholder, IsPassword = IsPassword,
			FontFamily = Theme.Body, FontSize = 14, TextColor = Theme.Ink, BackgroundColor = Colors.Transparent,
			HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center,
			Keyboard = KeyboardType switch { "email" => Keyboard.Email, "phone" => Keyboard.Telephone, "numeric" => Keyboard.Numeric, _ => Keyboard.Text }
		};

		var row = new Grid { ColumnSpacing = 10, ColumnDefinitions = { new(GridLength.Auto), new(GridLength.Star) } };
		if (!string.IsNullOrEmpty(Icon)) row.Add(new IconView { Icon = Icon, IconSize = 18, Color = Theme.InkSoft }, 0);
		row.Add(entry, 1);

		var box = new Border
		{
			MinimumHeightRequest = 52, StrokeThickness = 1, Stroke = Theme.Line, BackgroundColor = Theme.Paper,
			StrokeShape = Round(Theme.RadiusSm), Padding = new Thickness(15, 0), Content = row
		};
		entry.Focused += (_, _) => box.Stroke = Color.FromArgb("#4C9176");
		entry.Unfocused += (_, _) => box.Stroke = Theme.Line;

		Content = new VerticalStackLayout
		{
			Spacing = 8,
			Children = { Txt(Label, 12, Theme.BodyBold), box }
		};
	}
}
