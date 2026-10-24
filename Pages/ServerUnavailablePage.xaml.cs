using MobiloApp.Controls;

namespace MobiloApp.Pages;

public partial class ServerUnavailablePage : MobiloPage, IQueryAttributable
{
    string _retryRoute = "home";

    public ServerUnavailablePage() => InitializeComponent();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("retryRoute", out var route) && route is string value)
            _retryRoute = value is "search" ? value : "home";
    }

    async void OnRetry(object? sender, EventArgs e)
    {
        await Nav.Go(_retryRoute);
    }
}
