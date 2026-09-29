namespace plc.Plc;

/// <summary>
/// PLC 客户端接口，负责底层节点读写逻辑。
/// 通过此接口屏蔽具体 OPC UA 连接细节，便于后续切换到不同 PLC 协议。
/// </summary>
public interface IPlcClient
{
    /// <summary>
    /// 建立连接。
    /// </summary>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 当前是否已连接。
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 按节点 ID 读取值。
    /// </summary>
    Task<T> ReadValueAsync<T>(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取指定数据块的一段连续字节。
    /// </summary>
    Task<byte[]> ReadBlockAsync(int dbNumber, int startByte, int length, CancellationToken cancellationToken = default);

    /// <summary>
    /// 按节点 ID 写入值。
    /// </summary>
    Task WriteValueAsync<T>(string nodeId, T value, CancellationToken cancellationToken = default);
}
