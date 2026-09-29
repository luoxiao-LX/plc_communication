using System.Text.Json;

namespace plc.Plc;

/// <summary>
/// PLC 写入请求模型。
/// NodeId 表示目标节点地址，ValueType 表示写入值类型，Value 为实际写入参数。
/// </summary>
public class PlcWriteRequest
{
    public string NodeId { get; set; } = string.Empty;
    public string ValueType { get; set; } = "int";
    public JsonElement Value { get; set; }
}
