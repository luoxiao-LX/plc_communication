namespace plc.Plc;

/// <summary>
/// PLC 业务服务抽象层。
/// 定义通用的连接、读取和写入能力，供控制器和业务层统一调用。
/// </summary>
public interface IPlcService
{
    /// <summary>
    /// 建立与 PLC 的连接。
    /// </summary>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 当前是否已连接。
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 读取任意节点值，返回泛型结果。
    /// </summary>
    Task<T> ReadTagAsync<T>(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量读取多个节点值。
    /// 返回字典，key 为地址，value 为读取到的值。
    /// </summary>
    Task<Dictionary<string, object?>> ReadBatchAsync(IEnumerable<string> nodeIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取 DB 数据块中的连续字节。
    /// </summary>
    Task<byte[]> ReadBlockAsync(int dbNumber, int startByte, int length, CancellationToken cancellationToken = default);

    /// <summary>
    /// 写入任意节点值。
    /// </summary>
    Task WriteTagAsync<T>(string nodeId, T value, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取 16 位整数。
    /// </summary>
    Task<short> ReadInt16Async(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取 32 位整数。
    /// </summary>
    Task<int> ReadInt32Async(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取单精度浮点数。
    /// </summary>
    Task<float> ReadFloatAsync(string nodeId, CancellationToken cancellationToken = default);
}
