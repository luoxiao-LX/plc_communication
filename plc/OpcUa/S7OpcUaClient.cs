namespace plc.OpcUa;

/// <summary>
/// 兼容层：项目已切换为 Siemens S7 TCP 通信，不再使用 OPC UA。
/// 保留这个类作为历史兼容占位，避免旧代码编译失败。
/// </summary>
public class S7OpcUaClient
{
    public S7OpcUaClient(string serverUrl)
    {
    }

    public bool IsConnected => false;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<T> ReadValueAsync<T>(string nodeId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(default(T)!);
    }

    public Task WriteValueAsync<T>(string nodeId, T value, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
