namespace MobiloApp.Models;

public class Vehicle
{
	public int Id { get; init; }
	public string Name { get; init; } = "";
	public string Type { get; init; } = "";
	public string Rating { get; init; } = "";
	public int Reviews { get; init; }
	public decimal Price { get; init; }
	public string Image { get; init; } = "";
}

public class OnboardingSlide
{
	public string Eyebrow { get; init; } = "";
	public string Title { get; init; } = "";
	public string Body { get; init; } = "";
	public string Image { get; init; } = "";
}

public static class Sample
{
	const string Q = "?crop=entropy&cs=tinysrgb&fit=crop&fm=jpg&q=85&w=1200";
	public const string RangeImage = "https://images.unsplash.com/photo-1622268697621-54243bd1e9f4" + Q;
	public const string VolvoImage = "https://images.unsplash.com/photo-1519641471654-76ce0107ad1b" + Q;
	public const string FordImage = "https://images.unsplash.com/photo-1606103749141-1efed6903912" + Q;

	public static readonly List<Vehicle> Vehicles = new()
	{
		new() { Name = "Range Rover Velar", Type = "Luxury SUV",  Rating = "4.9", Reviews = 128, Price = 128, Image = RangeImage },
		new() { Name = "Volvo XC60",        Type = "Premium SUV", Rating = "4.8", Reviews = 96,  Price = 98,  Image = VolvoImage },
		new() { Name = "Ford Explorer",     Type = "Family SUV",  Rating = "4.7", Reviews = 74,  Price = 82,  Image = FordImage },
	};

	public static readonly List<OnboardingSlide> Slides = new()
	{
		new() { Eyebrow = "Discover",   Title = "Find the right vehicle for every journey.", Body = "Browse trusted cars near you, compare clearly, and choose with confidence.", Image = VolvoImage },
		new() { Eyebrow = "Effortless", Title = "Book instantly. No calls, no waiting.",     Body = "Choose your dates, see every cost up front, and reserve in just a few taps.", Image = RangeImage },
		new() { Eyebrow = "Protected",  Title = "Drive confidently, wherever you go.",       Body = "Verified hosts, flexible protection, and support that stays with you.", Image = FordImage },
	};
}
