using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

namespace MobiloApp.Controls;

/// <summary>
/// Base class for the design-system components. Each component declares bindable properties and
/// builds its visual tree in <see cref="Rebuild"/>, which re-runs whenever a property changes.
/// </summary>
public abstract class ControlBase : ContentView
{
	bool _ready;

	protected static BindableProperty Prop<TOwner, T>(string name, T defaultValue) =>
		BindableProperty.Create(name, typeof(T), typeof(TOwner), defaultValue,
			propertyChanged: (b, _, _) => ((ControlBase)b).Refresh());

	protected void Ready() { _ready = true; Rebuild(); }
	void Refresh() { if (_ready) Rebuild(); }
	protected abstract void Rebuild();

	protected static void Tap(View view, Action action)
	{
		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, _) => action();
		view.GestureRecognizers.Add(tap);
	}

	protected static Label Txt(string text, double size, string font = Theme.Body, Color? color = null) => new()
	{
		Text = text, FontSize = size, FontFamily = font, TextColor = color ?? Theme.Ink
	};

	protected static RoundRectangle Round(double r) => new() { CornerRadius = new CornerRadius(r) };
}

/// <summary>Base page: cream background, no Shell chrome, respects the iOS safe area.</summary>
public class MobiloPage : ContentPage
{
	public MobiloPage()
	{
		BackgroundColor = Theme.Cream;
		Shell.SetNavBarIsVisible(this, false);
		Shell.SetTabBarIsVisible(this, false);
		Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>(), true);
	}
}
