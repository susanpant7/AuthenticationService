namespace AuthenticationSystem.Responses;

public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message for client (success/info/error)
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The main payload (data)
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Optional metadata (paging, etc.)
    /// </summary>
    public object? Meta { get; set; }

    /// <summary>
    /// Timestamp of the response
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Convenience constructors
    public ApiResponse() { }

    public ApiResponse(T data, bool success = true, string? message = null, object? meta = null)
    {
        Data = data;
        Success = success;
        Message = message;
        Meta = meta;
    }

    // Static helpers
    public static ApiResponse<T> Ok(T data, string? message = null, object? meta = null)
        => new ApiResponse<T>(data, true, message, meta);

    public static ApiResponse<T> Fail(string message, object? meta = null)
        => new ApiResponse<T>(default!, false, message, meta);
}
