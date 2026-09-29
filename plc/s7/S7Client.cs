using S7.Net;

namespace plc.S7;

/// <summary>
/// Siemens S7 TCP 客户端封装。
/// 负责建立到 S7-1200/S7-1500 PLC 的连接，并提供统一的读写接口。
/// </summary>
public sealed class S7Client : IDisposable
{
    private readonly global::S7.Net.Plc _plc;

    public S7Client(string ip, int rack, int slot, global::S7.Net.CpuType cpuType = global::S7.Net.CpuType.S71200, int port = 102)
    {
        _plc = new global::S7.Net.Plc(cpuType, ip, port, (short)rack, (short)slot);
    }

    public bool IsConnected => _plc.IsConnected;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (IsConnected)
        {
            return Task.CompletedTask;
        }

        return Task.Run(() => _plc.Open(), cancellationToken);
    }

    public async Task<T> ReadValueAsync<T>(string variable, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("PLC is not connected.");
        }

        var value = await Task.Run(() => _plc.Read(variable), cancellationToken);
        if (value is null)
        {
            return default!;
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }

    public async Task<byte[]> ReadBlockAsync(int dbNumber, int startByte, int length, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("PLC is not connected.");
        }

        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero.");
        }

        return await Task.Run(() => _plc.ReadBytes(global::S7.Net.DataType.DataBlock, dbNumber, startByte, length), cancellationToken);
    }

    public async Task WriteValueAsync<T>(string variable, T value, CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("PLC is not connected.");
        }

        if (value is null)
        {
            throw new InvalidOperationException("PLC write value cannot be null.");
        }

        await Task.Run(() => _plc.Write(variable, value), cancellationToken);
    }

    public void Dispose()
    {
        if (_plc.IsConnected)
        {
            _plc.Close();
        }
    }
}
