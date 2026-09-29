namespace plc.Plc;

/// <summary>
/// 统一 API 返回结构。
/// 统一返回 Success、Message、Data 三个字段，便于前端统一处理成功/失败。
/// </summary>
public class ApiResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    /// <summary>
    /// 构造成功响应。
    /// </summary>
    public static ApiResult<T> Ok(T? data, string message = "success")
    {
        return new ApiResult<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 构造失败响应。
    /// </summary>
    public static ApiResult<T> Fail(string message, T? data = default)
    {
        return new ApiResult<T>
        {
            Success = false,
            Message = message,
            Data = data
        };
    }
}
