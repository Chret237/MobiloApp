using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace MobiloApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				// Manrope = display font (headings, logo, prices). DM Sans = body font.
				fonts.AddFont("Manrope-Bold.ttf", "ManropeBold");
				fonts.AddFont("Manrope-ExtraBold.ttf", "ManropeExtraBold");
				fonts.AddFont("DMSans-Regular.ttf", "DMSans");
				fonts.AddFont("DMSans-Medium.ttf", "DMSansMedium");
				fonts.AddFont("DMSans-SemiBold.ttf", "DMSansSemiBold");
				fonts.AddFont("DMSans-Bold.ttf", "DMSansBold");
			});

		// The design draws its own border/background around inputs (Field control),
		// so remove the native underline / border of Entry.
		EntryHandler.Mapper.AppendToMapping("MobiloBorderless", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.Background = null;
			handler.PlatformView.SetPadding(0, 0, 0, 0);
#elif IOS
			handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
		});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
