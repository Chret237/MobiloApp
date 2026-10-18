using Microsoft.Maui.Controls.Shapes;
using MobiloApp.Controls;
using MobiloApp.Models;

namespace MobiloApp.Pages;

public partial class OnboardingPage : MobiloPage
{
	int _index;

	public OnboardingPage() { InitializeComponent(); Render(); }

	protected override void OnAppearing() { base.OnAppearing(); _index = 0; Render(); }

	void Render()
	{
		var slide = Sample.Slides[_index];
		HeroImage.Source = slide.Image;
		EyebrowLabel.Text = slide.Eyebrow;
		TitleLabel.Text = slide.Title;
		BodyLabel.Text = slide.Body;

		Dots.Children.Clear();
		for (var i = 0; i < Sample.Slides.Count; i++)
			Dots.Children.Add(new Border
			{
				HeightRequest = 7, WidthRequest = i == _index ? 26 : 7, StrokeThickness = 0,
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) },
				BackgroundColor = i == _index ? Theme.Forest : Theme.LineDark
			});
	}

	async void OnNext(object? sender, TappedEventArgs e)
	{
		if (_index == Sample.Slides.Count - 1) await Nav.Go("login");
		else { _index++; Render(); }
	}

	async void OnSkip(object? sender, TappedEventArgs e) => await Nav.Go("login");
}
