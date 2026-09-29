namespace plc.Plc;

/// <summary>
/// PLC 连续内存字节解析辅助类。
/// 适用于从 <see cref="PlcService.ReadBlockAsync(int, int, int, CancellationToken)"/> 读取到的原始 byte[] 中
/// 还原成常见的 PLC 数据类型，例如 short、int、float。
/// 
/// 注意：
/// 1. Siemens S7 PLC 在内存中通常采用小端字节序。
/// 2. 读取的 byte[] 是原始数据块内容，不包含任何额外的长度标识或类型信息。
/// 3. 调用方应根据真实数据块布局自行决定从哪个 offset 开始解析。
/// </summary>
public static class PlcByteParser
{
    /// <summary>
    /// 从字节数组中读取一个 16 位有符号整数（short）。
    /// 常用于解析 PLC 中的 WORD / INT 型变量。
    /// </summary>
    /// <param name="bytes">原始字节数组。</param>
    /// <param name="offset">起始偏移量，单位为字节。</param>
    /// <returns>解析后的 short 值。</returns>
    public static short ToInt16(byte[] bytes, int offset = 0)
    {
        EnsureValidRange(bytes, offset, sizeof(short));
        return BitConverter.ToInt16(bytes, offset);
    }

    /// <summary>
    /// 从字节数组中读取一个 32 位有符号整数（int）。
    /// 常用于解析 PLC 中的 DINT / DWORD / INT32 型变量。
    /// </summary>
    /// <param name="bytes">原始字节数组。</param>
    /// <param name="offset">起始偏移量，单位为字节。</param>
    /// <returns>解析后的 int 值。</returns>
    public static int ToInt32(byte[] bytes, int offset = 0)
    {
        EnsureValidRange(bytes, offset, sizeof(int));
        return BitConverter.ToInt32(bytes, offset);
    }

    /// <summary>
    /// 从字节数组中读取一个 32 位单精度浮点数（float）。
    /// 常用于解析 PLC 中的 REAL / FLOAT 型变量。
    /// </summary>
    /// <param name="bytes">原始字节数组。</param>
    /// <param name="offset">起始偏移量，单位为字节。</param>
    /// <returns>解析后的 float 值。</returns>
    public static float ToSingle(byte[] bytes, int offset = 0)
    {
        EnsureValidRange(bytes, offset, sizeof(float));
        return BitConverter.ToSingle(bytes, offset);
    }

    /// <summary>
    /// 从字节数组中读取一组连续的 short 值。
    /// 例如：从 DB1 中读取 10 个 WORD，则可将结果按 2 字节为单位依次解析出来。
    /// </summary>
    /// <param name="bytes">原始字节数组。</param>
    /// <param name="offset">起始偏移量，单位为字节。</param>
    /// <param name="count">要解析的 short 数量。</param>
    /// <returns>转换后的 short 数组。</returns>
    public static short[] ToInt16Array(byte[] bytes, int offset = 0, int count = 1)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "count 必须大于 0。");
        }

        var result = new short[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = ToInt16(bytes, offset + i * sizeof(short));
        }

        return result;
    }

    /// <summary>
    /// 统一进行安全性校验，确保 offset + size 在数组边界内。
    /// </summary>
    private static void EnsureValidRange(byte[] bytes, int offset, int size)
    {
        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes), "bytes 不能为 null。");
        }

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "offset 不能小于 0。");
        }

        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "size 必须大于 0。");
        }

        if (offset + size > bytes.Length)
        {
            throw new InvalidOperationException($"字节读取越界：offset={offset}, size={size}, bytes.Length={bytes.Length}。");
        }
    }
}
