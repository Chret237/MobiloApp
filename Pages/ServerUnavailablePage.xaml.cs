using MobiloApp.Controls;

namespace MobiloApp.Pages;

public partial class ServerUnavailablePage : MobiloPage, IQueryAttributable
{
    string _retryRoute = "home";
    int? _vehicleId;

    public ServerUnavailablePage() => InitializeComponent();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("retryRoute", out var route) && route is string value)
            _retryRoute = value is "search" or "details" ? value : "home";
        if (query.TryGetValue("vehicleId", out var vehicleId) && int.TryParse(vehicleId?.ToString(), out var id))
            _vehicleId = id;
    }

    async void OnRetry(object? sender, EventArgs e)
    {
        if (_retryRoute == "details" && _vehicleId is not null)
        {
            await Nav.Go(_retryRoute, new Dictionary<string, object>
            {
                ["vehicleId"] = _vehicleId.Value
            });
            return;
        }

        await Nav.Go(_retryRoute);
    }
}
