namespace sprint19_MinimalAPI.Common;

public static class ApiResponse
{
    public static object Success<T>(T data) => new { success = true, data };
    public static object Error(string message) => new { success = false, errors = new[] { message } };
    public static object Error(IEnumerable<string> errors) => new { success = false, errors };
}