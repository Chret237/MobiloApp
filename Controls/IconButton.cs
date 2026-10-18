using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MobiloApp.Controls;

/// <summary>Circular icon button (IconButton component). Glass = translucent variant for dark backgrounds.</summary>
public class IconButton : ControlBase
{
	public static readonly BindableProperty IconProperty = Prop<IconButton, string>(nameof(Icon), "");
	public static readonly BindableProperty ActiveProperty = Prop<IconButton, bool>(nameof(Active), false);
	public static readonly BindableProperty GlassProperty = Prop<IconButton, bool>(nameof(Glass), false);
	public static readonly BindableProperty ButtonSizeProperty = Prop<IconButton, double>(nameof(ButtonSize), 42d);
	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(IconButton));

	public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
	public bool Active { get => (bool)GetValue(ActiveProperty); set => SetValue(ActiveProperty, value); }
	public bool Glass { get => (bool)GetValue(GlassProperty); set => SetValue(GlassProperty, value); }
	public double ButtonSize { get => (double)GetValue(ButtonSizeProperty); set => SetValue(ButtonSizeProperty, value); }
	public ICommand? Command { get => (ICommand?)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
	public event EventHandler? Clicked;

	public IconButton() { HorizontalOptions = VerticalOptions = LayoutOptions.Center; Ready(); }

	protected override void Rebuild()
	{
		WidthRequest = HeightRequest = ButtonSize;
		var fg = Active || Glass ? Colors.White : Theme.Ink;
		var border = new Border
		{
			StrokeShape = new Ellipse(), StrokeThickness = Active ? 0 : 1,
			Stroke = Glass ? Colors.White.WithAlpha(.18f) : Theme.Line,
			BackgroundColor = Active ? Theme.Forest : Glass ? Colors.White.WithAlpha(.12f) : Colors.White.WithAlpha(.92f),
			Content = new IconView { Icon = Icon, IconSize = ButtonSize >= 36 ? 20 : 16, Color = fg }
		};
		SemanticProperties.SetDescription(border, Icon);
		Tap(border, () => { Clicked?.Invoke(this, EventArgs.Empty); Command?.Execute(null); });
		Content = border;
	}
}
