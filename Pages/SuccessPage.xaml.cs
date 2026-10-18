using MobiloApp.Controls;

namespace MobiloApp.Pages;

public partial class SuccessPage : MobiloPage
{
	public SuccessPage() => InitializeComponent();

	async void OnViewBooking(object? sender, EventArgs e) => await Nav.Go("bookings");
	async void OnHome(object? sender, TappedEventArgs e) => await Nav.Go("home");
}
