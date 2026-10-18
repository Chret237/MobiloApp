using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>"Where do you need a car?" pill (SearchBox component).</summary>
public class SearchBox : ControlBase
{
	public static readonly BindableProperty CompactProperty = Prop<SearchBox, bool>(nameof(Compact), false);
	public bool Compact { get => (bool)GetValue(CompactProperty); set => SetValue(CompactProperty, value); }
	public event EventHandler? Tapped;

	public SearchBox() => Ready();

	protected override void Rebuild()
	{
		Margin = new Thickness(22, Compact ? 2 : 8, 22, 14);
		var row = new Grid { ColumnSpacing = 10, ColumnDefinitions = { new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto) } };
		row.Add(new IconView { Icon = "search", Color = Theme.InkSoft }, 0);
		row.Add(new Label { Text = "Where do you need a car?", TextColor = Theme.InkSoft, VerticalOptions = LayoutOptions.Center }, 1);
		if (!Compact)
		{
			row.Add(new Border
			{
				WidthRequest = 38, HeightRequest = 38, StrokeShape = Round(9), StrokeThickness = 0, BackgroundColor = Theme.Mint,
				Content = new IconView { Icon = "filter", IconSize = 17, Color = Theme.Forest }
			}, 2);
		}

		var box = new Border
		{
			HeightRequest = Compact ? 50 : 58, StrokeShape = Round(Theme.RadiusSm), BackgroundColor = Colors.White,
			StrokeThickness = Compact ? 1 : 0, Stroke = Theme.Line, Padding = new Thickness(17, 0, 10, 0),
			Shadow = Compact ? null! : Theme.SoftShadow, Content = row
		};
		Tap(box, () => Tapped?.Invoke(this, EventArgs.Empty));
		Content = box;
	}
}
