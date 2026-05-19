namespace Rheum.WebApi;

internal static class HealthRequestContext
{
    private static readonly AsyncLocal<bool> _isHealthRequest = new();

    public static bool IsHealthRequest
    {
        get => _isHealthRequest.Value;
        set => _isHealthRequest.Value = value;
    }
}
