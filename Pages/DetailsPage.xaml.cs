using MobiloApp.Controls;
using MobiloApp.Models;

namespace MobiloApp.Pages;

public partial class DetailsPage : MobiloPage
{
	public DetailsPage()
	{
		InitializeComponent();
		HeroImage.Source = Sample.RangeImage;
		SizeChanged += (_, _) => Hero.HeightRequest = Math.Max(330, Height * 0.43); // 43dvh, min 330
	}

	async void OnBack(object? sender, EventArgs e) => await Nav.Go("search");
	async void OnBook(object? sender, EventArgs e) => await Nav.Go("booking");
}
