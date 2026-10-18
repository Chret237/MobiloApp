using MobiloApp.Controls;
using MobiloApp.Models;

namespace MobiloApp.Pages;

/// <summary>Placeholder tab screens (UtilityScreen in the design): Bookings, Favorites and Profile.</summary>
public partial class UtilityPage : MobiloPage
{
	public UtilityPage(string kind)
	{
		InitializeComponent();
		var (title, body, icon) = kind switch
		{
			"bookings" => ("Your bookings", "Your upcoming trip is ready.", "calendar"),
			"favorites" => ("Saved vehicles", "Keep your favorite rides close.", "heart"),
			_ => ("Your profile", "Manage account, payments, and preferences.", "user"),
		};
		Header.Title = title;
		BigIcon.Icon = icon;
		BodyTitle.Text = body;
		BottomBar.Active = kind;

		var bookings = kind == "bookings";
		UpcomingCard.IsVisible = bookings;
		BodyText.IsVisible = !bookings;
		BodyText.Text = kind == "favorites" ? "Tap the heart on any vehicle to save it here." : "Alex Morgan · Member since 2024";
		if (bookings) CarImage.Source = Sample.RangeImage;
	}
}

public class BookingsPage : UtilityPage { public BookingsPage() : base("bookings") { } }
public class FavoritesPage : UtilityPage { public FavoritesPage() : base("favorites") { } }
public class ProfilePage : UtilityPage { public ProfilePage() : base("profile") { } }
