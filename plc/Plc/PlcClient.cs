using plc.S7;

namespace plc.Plc;

/// <summary>
/// 用于包装 Siemens S7 底层实现的 PLC 客户端适配器。
/// 这样业务代码只依赖 IPlcClient，而不直接依赖 S7 具体实现细节。
/// </summary>
public class PlcClient : IPlcClient
{
    private readonly S7Client _s7Client;

    public PlcClient(S7Client s7Client)
    {
        _s7Client = s7Client;
    }

    public bool IsConnected => _s7Client.IsConnected;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return _s7Client.ConnectAsync(cancellationToken);
    }

    public Task<T> ReadValueAsync<T>(string nodeId, CancellationToken cancellationToken = default)
    {
        return _s7Client.ReadValueAsync<T>(nodeId, cancellationToken);
    }

    public Task<byte[]> ReadBlockAsync(int dbNumber, int startByte, int length, CancellationToken cancellationToken = default)
    {
        return _s7Client.ReadBlockAsync(dbNumber, startByte, length, cancellationToken);
    }

    public Task WriteValueAsync<T>(string nodeId, T value, CancellationToken cancellationToken = default)
    {
        return _s7Client.WriteValueAsync(nodeId, value, cancellationToken);
    }
}
