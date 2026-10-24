# MOBILO – .NET MAUI Car-rental App

UI: Splash → Onboarding → Login/Register → Home → Search → Details → Booking → Payment → Success,
plus Bookings / Favorites / Profile tabs. 13 routes, all wired in `AppShell.xaml`.

## Run

1. Install the .NET 10 SDK + the MAUI workload: `dotnet workload install maui`
2. To start the API and launch the Android app together, run PowerShell from the repository root:

   `./run-dev.ps1`

   The script starts the API, configures the Android bridge, then launches `net10.0-android`. Keep the terminal open while using the app; press Enter after closing the app to stop the API. You can select another target with `./run-dev.ps1 -Framework net10.0-ios`.
3. To run only the API: `dotnet run --project backend/Mobilo.Api/Mobilo.Api.csproj --urls http://localhost:5080`

The MAUI home and search screens load their vehicles from `GET /api/vehicles`. Android emulator traffic uses `http://10.0.2.2:5080`; iOS simulator traffic uses `http://localhost:5080`. A physical Android phone uses `http://127.0.0.1:5080` through the automatic `adb reverse` bridge.

For a physical Android phone, enable USB debugging, connect it by USB, accept the debugging prompt, and ensure `adb` is available from the Android SDK platform-tools. `run-dev.ps1` configures `adb reverse tcp:5080 tcp:5080` automatically, so the project does not contain a machine-specific IP address or require a firewall rule.

For the first Android build, NuGet must be able to download the MAUI AndroidX packages.
If the assets are not restored yet, run `dotnet restore MobiloApp.csproj` once before using the combined script.

iOS needs a Mac. If you use a different SDK, update the `net10.0-*` target frameworks in the csproj accordingly.

Vehicle photos load from Unsplash, so the device needs internet.
