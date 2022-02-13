namespace Entitys.Web;

public class ModelStateResult<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
}
public class ModelStateResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? Message { get; set; }
}