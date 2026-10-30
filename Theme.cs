namespace MobiloApp;

/// <summary>Design tokens for code-built controls (mirror of Resources/Styles/Colors.xaml).</summary>
public static class Theme
{
	public static readonly Color Ink = Color.FromArgb("#15231E");
	public static readonly Color InkSoft = Color.FromArgb("#50615A");
	public static readonly Color Forest = Color.FromArgb("#173F35");
	public static readonly Color ForestDeep = Color.FromArgb("#0D2C25");
	public static readonly Color Mint = Color.FromArgb("#DFF2E8");
	public static readonly Color MintBright = Color.FromArgb("#A9E6C9");
	public static readonly Color Cream = Color.FromArgb("#F6F4EE");
	public static readonly Color Paper = Colors.White;
	public static readonly Color Line = Color.FromArgb("#DFE5E1");
	public static readonly Color LineDark = Color.FromArgb("#CBD4CF");
	public static readonly Color Gold = Color.FromArgb("#EFAD43");
	public static readonly Color Accent = Color.FromArgb("#367C63");
	public static readonly Color Muted = Color.FromArgb("#809088");
	public static readonly Color Placeholder = Color.FromArgb("#91A099");

	public const string Display = "ManropeBold";
	public const string DisplayHeavy = "ManropeExtraBold";
	public const string Body = "DMSans";
	public const string BodyMedium = "DMSansMedium";
	public const string BodySemi = "DMSansSemiBold";
	public const string BodyBold = "DMSansBold";

	public const double RadiusSm = 12;
	public const double Radius = 18;
	public const double RadiusLg = 28;

	public static Shadow SoftShadow => new() { Brush = Brush.Black, Opacity = .08f, Radius = 16, Offset = new Point(0, 8) };
	public static Shadow ButtonShadow => new() { Brush = new SolidColorBrush(Forest), Opacity = .22f, Radius = 12, Offset = new Point(0, 8) };
}

/// <summary>Navigation helper: every screen is a Shell route.</summary>
public static class Nav
{
	public static Task Go(string route) => Shell.Current.GoToAsync("//" + route);
	public static Task Go(string route, IDictionary<string, object> parameters) =>
		Shell.Current.GoToAsync("//" + route, parameters);
}

/// <summary>Booking data shared between the booking, payment and success screens.</summary>
public static class BookingState
{
	public static int? VehicleId { get; set; }
	public static bool Insurance { get; set; } = true;
	public static int Total => Insurance ? 526 : 482;
}
