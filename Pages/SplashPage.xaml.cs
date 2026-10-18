using MobiloApp.Controls;

namespace MobiloApp.Pages;

public partial class SplashPage : MobiloPage
{
	public SplashPage() => InitializeComponent();

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		LoaderFill.WidthRequest = 0;
		await Task.Delay(50); // let the track get its width

		var width = LoaderTrack.Width > 0 ? LoaderTrack.Width : 280;
		new Animation(v => LoaderFill.WidthRequest = v, 0, width, Easing.CubicInOut)
			.Commit(this, "load", 16, 2000);

		await Task.Delay(2000);
		await Nav.Go("onboarding");
	}
}
