namespace Blazor.Services;

public class ApiResult<TData>
{
    
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public required string Message { get; init; } = null!;
    
    public TData? Data { get; set; }

}