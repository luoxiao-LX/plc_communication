namespace plc.S7;

/// <summary>
/// Siemens S7 TCP 连接配置。
/// 该配置用于直接连接 PLC，不再依赖 OPC UA 作为通信协议。
/// </summary>
public class S7Options
{
    public string Ip { get; set; } = "192.168.0.1";
    public int Port { get; set; } = 102;
    public int Rack { get; set; } = 0;
    public int Slot { get; set; } = 0;
    public string CpuType { get; set; } = "S71200";
    public int ReadTimeoutMs { get; set; } = 3000;
    public int WriteTimeoutMs { get; set; } = 3000;
}
