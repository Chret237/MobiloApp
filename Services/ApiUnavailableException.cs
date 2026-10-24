namespace MobiloApp.Services;

public sealed class ApiUnavailableException(string message, Exception innerException)
    : Exception(message, innerException);
