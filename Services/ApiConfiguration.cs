namespace MobiloApp.Services;

public static class ApiConfiguration
{
    public static Uri BaseUri => DeviceInfo.Platform == DevicePlatform.Android
        ? new Uri(DeviceInfo.Current.DeviceType == DeviceType.Physical
            ? "http://127.0.0.1:5080/"
            : "http://10.0.2.2:5080/")
        : new Uri("http://localhost:5080/");
}
