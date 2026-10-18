# MOBILO – .NET MAUI Car-rental App

UI: Splash → Onboarding → Login/Register → Home → Search → Details → Booking → Payment → Success,
plus Bookings / Favorites / Profile tabs. 13 routes, all wired in `AppShell.xaml`.

## Run
1. Install the .NET 10 SDK + the MAUI workload: `dotnet workload install maui`
2. `dotnet build -t:Run -f net10.0-android` (or open `MobiloApp.csproj` in Visual Studio / VS Code and pick a device)
   - iOS needs a Mac. If you use a different SDK, update the `net10.0-*` target frameworks in the csproj accordingly.
3. Vehicle photos load from Unsplash, so the device needs internet.

