using MobiloApp.Controls;
using MobiloApp.Models;

namespace MobiloApp.Pages;

public partial class BookingPage : MobiloPage
{
	public BookingPage()
	{
		InitializeComponent();
		CarImage.Source = Sample.RangeImage;
		UpdateTotals();
	}

	protected override void OnAppearing() { base.OnAppearing(); InsuranceSwitch.IsToggled = BookingState.Insurance; UpdateTotals(); }

	void UpdateTotals()
	{
		ProtectionRow.IsVisible = BookingState.Insurance;
		TotalLabel.Text = FooterTotal.Text = $"${BookingState.Total}";
	}

	void OnInsuranceToggled(object? sender, ToggledEventArgs e) { BookingState.Insurance = e.Value; UpdateTotals(); }
	async void OnBack(object? sender, EventArgs e) => await Nav.Go("details");
	async void OnContinue(object? sender, EventArgs e) => await Nav.Go("payment");
}
