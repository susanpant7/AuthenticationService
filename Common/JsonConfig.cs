using System.Text.Json;

namespace AuthenticationSystem.Common;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}