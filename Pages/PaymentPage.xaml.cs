using Microsoft.Maui.Controls.Shapes;
using MobiloApp.Controls;

namespace MobiloApp.Pages;

public partial class PaymentPage : MobiloPage
{
	static readonly (string Id, string Icon, string Title, string Sub)[] MethodList =
	{
		("card", "card", "Credit or debit card", "Visa, Mastercard, Amex"),
		("mobile", "phone", "Mobile money", "Fast payment from your phone"),
		("wallet", "wallet", "MOBILO Wallet", "$620 available"),
	};

	string _method = "card";

	public PaymentPage() { InitializeComponent(); BuildMethods(); }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var total = $"${BookingState.Total}.00";
		TotalLabel.Text = total;
		PayButton.Text = $"Pay {total}";
	}

	void BuildMethods()
	{
		Methods.Children.Clear();
		CardForm.IsVisible = _method == "card";

		foreach (var (id, icon, title, sub) in MethodList)
		{
			var active = _method == id;

			var radio = new Border
			{
				WidthRequest = 17, HeightRequest = 17, StrokeShape = new Ellipse(), StrokeThickness = 1,
				Stroke = active ? Theme.Forest : Theme.LineDark, Padding = 3, BackgroundColor = Colors.Transparent,
				Content = active ? new Ellipse { Fill = new SolidColorBrush(Theme.Forest) } : null
			};
			var row = new Grid { ColumnSpacing = 11, ColumnDefinitions = { new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto) } };
			row.Add(new Border
			{
				WidthRequest = 42, HeightRequest = 42, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
				StrokeThickness = 0, BackgroundColor = Theme.Mint, Content = new IconView { Icon = icon, Color = Theme.Forest }
			}, 0);
			row.Add(new VerticalStackLayout
			{
				Spacing = 2, VerticalOptions = LayoutOptions.Center,
				Children =
				{
					new Label { Text = title, FontFamily = Theme.BodyBold, FontSize = 11 },
					new Label { Text = sub, FontSize = 9, TextColor = Theme.InkSoft }
				}
			}, 1);
			row.Add(radio, 2);

			var option = new Border
			{
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(Theme.RadiusSm) }, Padding = 10, StrokeThickness = 1,
				Stroke = active ? Color.FromArgb("#4A8E73") : Theme.Line, BackgroundColor = Colors.White, Content = row
			};
			var tap = new TapGestureRecognizer();
			var captured = id;
			tap.Tapped += (_, _) => { _method = captured; BuildMethods(); };
			option.GestureRecognizers.Add(tap);
			Methods.Children.Add(option);
		}
	}

	async void OnBack(object? sender, EventArgs e) => await Nav.Go("booking");
	async void OnReview(object? sender, TappedEventArgs e) => await Nav.Go("booking");
	async void OnPay(object? sender, EventArgs e) => await Nav.Go("success");
}
