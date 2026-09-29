namespace plc.Plc;

/// <summary>
/// 连续数据块读取请求模型。
/// 用于从 PLC 的指定 DB 数据块中连续读取若干字节，再在业务层自行按 short/int/float 等类型解析。
/// 示例：
/// {
///   "dbNumber": 1,
///   "startByte": 0,
///   "length": 16
/// }
/// 表示从 DB1 的字节偏移 0 开始读取 16 个字节。
/// </summary>
public class PlcReadBlockRequest
{
    /// <summary>
    /// 要读取的数据块编号。例如：DB1 则写 1。
    /// </summary>
    public int DbNumber { get; set; }

    /// <summary>
    /// 读取开始位置的字节偏移，0 表示从 DB 开头读取。
    /// </summary>
    public int StartByte { get; set; }

    /// <summary>
    /// 读取的总长度，单位为字节。
    /// </summary>
    public int Length { get; set; } = 16;
}
