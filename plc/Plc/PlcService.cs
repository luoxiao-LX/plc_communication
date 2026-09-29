using plc.S7;

namespace plc.Plc;

/// <summary>
/// PLC 业务服务实现。
/// 对底层 IPlcClient 做一层包装，提供更贴近业务使用的读取和写入方法。
/// </summary>
public class PlcService : IPlcService
{
    private readonly IPlcClient _client;

    public PlcService(IPlcClient client)
    {
        _client = client;
    }

    public bool IsConnected => _client.IsConnected;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return _client.ConnectAsync(cancellationToken);
    }

    public async Task<T> ReadTagAsync<T>(string nodeId, CancellationToken cancellationToken = default)
    {
        return await _client.ReadValueAsync<T>(nodeId, cancellationToken);
    }

    public async Task<Dictionary<string, object?>> ReadBatchAsync(IEnumerable<string> nodeIds, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, object?>();

        foreach (var nodeId in nodeIds)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                continue;
            }

            var value = await _client.ReadValueAsync<object>(nodeId, cancellationToken);
            result[nodeId] = value;
        }

        return result;
    }

    public Task<byte[]> ReadBlockAsync(int dbNumber, int startByte, int length, CancellationToken cancellationToken = default)
    {
        return _client.ReadBlockAsync(dbNumber, startByte, length, cancellationToken);
    }

    public Task WriteTagAsync<T>(string nodeId, T value, CancellationToken cancellationToken = default)
    {
        return _client.WriteValueAsync(nodeId, value, cancellationToken);
    }

    public async Task<short> ReadInt16Async(string nodeId, CancellationToken cancellationToken = default)
    {
        var value = await _client.ReadValueAsync<short>(nodeId, cancellationToken);
        return value;
    }

    public async Task<int> ReadInt32Async(string nodeId, CancellationToken cancellationToken = default)
    {
        var value = await _client.ReadValueAsync<int>(nodeId, cancellationToken);
        return value;
    }

    public async Task<float> ReadFloatAsync(string nodeId, CancellationToken cancellationToken = default)
    {
        var value = await _client.ReadValueAsync<float>(nodeId, cancellationToken);
        return value;
    }
}
