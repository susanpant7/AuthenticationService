namespace AuthenticationSystem.Common;

public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Title for client (success/info/error)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// HTTP status code or custom status identifier
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Collection of error messages or validation errors
    /// </summary>
    public IEnumerable<string>? Errors { get; set; }

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

    public ApiResponse(T data, bool success = true, string? title = null, int status = 200, IEnumerable<string>? errors = null, object? meta = null)
    {
        Data = data;
        Success = success;
        Title = title;
        Status = status;
        Errors = errors;
        Meta = meta;
    }

    // Static helpers
    public static ApiResponse<T> SuccessResponse(T data, string title = "Api Request Success", int status = 200, object? meta = null)
        => new ApiResponse<T>(data, true, title, status, null, meta);

    public static ApiResponse<T> ErrorResponse(string title, int status = 400, IEnumerable<string>? errors = null, object? meta = null)
        => new ApiResponse<T>(default!, false, title, status, errors, meta);
}