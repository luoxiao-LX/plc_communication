namespace plc.OpcUa;

/// <summary>
/// OPC UA 连接配置。
/// ServerUrl 是 PLC 的 OPC UA 服务地址，通常是 opc.tcp://192.168.0.1:4840。
/// </summary>
public class OpcUaOptions
{
    public string ServerUrl { get; set; } = "opc.tcp://192.168.0.1:4840";
    public int SessionTimeoutMs { get; set; } = 60000;
    public bool UseSecurity { get; set; } = false;
    public string DefaultNodePrefix { get; set; } = "ns=3;s=";
}
