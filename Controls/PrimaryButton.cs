using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>Full-width button (Button component). Variant: primary | secondary | ghost.</summary>
public class PrimaryButton : ControlBase
{
	public static readonly BindableProperty TextProperty = Prop<PrimaryButton, string>(nameof(Text), "");
	public static readonly BindableProperty IconProperty = Prop<PrimaryButton, string>(nameof(Icon), "");
	public static readonly BindableProperty VariantProperty = Prop<PrimaryButton, string>(nameof(Variant), "primary");

	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public string Variant { get => (string)GetValue(VariantProperty); set => SetValue(VariantProperty, value); }
	public event EventHandler? Clicked;

	public PrimaryButton() => Ready();

	protected override void Rebuild()
	{
		var primary = Variant == "primary";
		var fg = primary ? Colors.White : Theme.Forest;
		var row = new HorizontalStackLayout { Spacing = 14, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
		row.Add(new Label { Text = Text, FontFamily = Theme.BodyBold, FontSize = 15, TextColor = fg, VerticalOptions = LayoutOptions.Center });
		if (!string.IsNullOrEmpty(Icon)) row.Add(new IconView { Icon = Icon, IconSize = 20, Color = fg });

		var border = new Border
		{
			MinimumHeightRequest = 54, HeightRequest = 54, StrokeThickness = 0, StrokeShape = Round(Theme.RadiusSm),
			Padding = new Thickness(20, 0),
			BackgroundColor = primary ? Theme.Forest : Variant == "secondary" ? Theme.Mint : Colors.Transparent,
			Shadow = primary ? Theme.ButtonShadow : null!,
			Content = row
		};
		Tap(border, async () =>
		{
			await border.ScaleTo(.985, 60); await border.ScaleTo(1, 60);
			Clicked?.Invoke(this, EventArgs.Empty);
		});
		Content = border;
	}
}
