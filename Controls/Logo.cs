using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>MOBILO wordmark with the car mark (Logo component).</summary>
public class Logo : ControlBase
{
	public static readonly BindableProperty LightProperty = Prop<Logo, bool>(nameof(Light), false);
	public bool Light { get => (bool)GetValue(LightProperty); set => SetValue(LightProperty, value); }

	public Logo() { HorizontalOptions = LayoutOptions.Start; Ready(); }

	protected override void Rebuild()
	{
		var mark = new Border
		{
			WidthRequest = 39, HeightRequest = 39, StrokeThickness = 0, StrokeShape = Round(11),
			BackgroundColor = Light ? Colors.White.WithAlpha(.16f) : Theme.Forest,
			Content = new IconView { Icon = "car", IconSize = 22, Color = Colors.White }
		};
		Content = new HorizontalStackLayout
		{
			Spacing = 9, VerticalOptions = LayoutOptions.Center,
			Children =
			{
				mark,
				new Label
				{
					Text = "MOBILO", FontSize = 18, FontFamily = Theme.DisplayHeavy, CharacterSpacing = 2.2,
					TextColor = Light ? Colors.White : Theme.Ink, VerticalOptions = LayoutOptions.Center
				}
			}
		};
	}
}
