using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>Small uppercase pill (Badge component). Mint = filled mint variant, otherwise translucent white.</summary>
public class Badge : ControlBase
{
	public static readonly BindableProperty TextProperty = Prop<Badge, string>(nameof(Text), "");
	public static readonly BindableProperty MintProperty = Prop<Badge, bool>(nameof(Mint), false);
	public static readonly BindableProperty BadgeFontSizeProperty = Prop<Badge, double>(nameof(BadgeFontSize), 10d);

	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
	public bool Mint { get => (bool)GetValue(MintProperty); set => SetValue(MintProperty, value); }
	public double BadgeFontSize { get => (double)GetValue(BadgeFontSizeProperty); set => SetValue(BadgeFontSizeProperty, value); }

	public Badge() { HorizontalOptions = LayoutOptions.Start; Ready(); }

	protected override void Rebuild()
	{
		Content = new Border
		{
			StrokeShape = Round(999), StrokeThickness = Mint ? 0 : 1, Stroke = Colors.White.WithAlpha(.5f),
			BackgroundColor = Mint ? Theme.Mint : Colors.White.WithAlpha(.9f), Padding = new Thickness(9, 6),
			Content = new Label
			{
				Text = Text, FontFamily = Theme.BodyBold, FontSize = BadgeFontSize, TextColor = Theme.Forest,
				CharacterSpacing = .4, TextTransform = TextTransform.Uppercase
			}
		};
	}
}
