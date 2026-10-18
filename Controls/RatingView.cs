namespace MobiloApp.Controls;

/// <summary>Gold star + score + optional review count (Rating component).</summary>
public class RatingView : ControlBase
{
	public static readonly BindableProperty ValueProperty = Prop<RatingView, string>(nameof(Value), "");
	public static readonly BindableProperty CountProperty = Prop<RatingView, int>(nameof(Count), 0);

	public string Value { get => (string)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
	public int Count { get => (int)GetValue(CountProperty); set => SetValue(CountProperty, value); }

	public RatingView() { HorizontalOptions = LayoutOptions.Start; Ready(); }

	protected override void Rebuild()
	{
		var row = new HorizontalStackLayout { Spacing = 4, VerticalOptions = LayoutOptions.Center };
		row.Add(new IconView { Icon = "star", IconSize = 14, Color = Theme.Gold, Filled = true });
		row.Add(new Label { Text = Value, FontFamily = Theme.BodyBold, FontSize = 11, VerticalOptions = LayoutOptions.Center });
		if (Count > 0)
			row.Add(new Label { Text = $"({Count})", FontSize = 11, TextColor = Theme.InkSoft, VerticalOptions = LayoutOptions.Center });
		Content = row;
	}
}
