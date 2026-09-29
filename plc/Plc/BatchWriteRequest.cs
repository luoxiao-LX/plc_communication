namespace plc.Plc;

/// <summary>
/// 标准的批量写入请求模型。
/// 该模型将多个单节点 PLC 写入请求封装为一个完整的批量 payload，便于前端/调用方一次性提交多个地址写命令。
/// 典型使用方式：
/// {
///   "items": [
///     { "nodeId": "DB1.DBW0", "valueType": "short", "value": 10 },
///     { "nodeId": "DB1.DBX6.0", "valueType": "bool", "value": true }
///   ]
/// }
/// </summary>
public class BatchWriteRequest
{
    /// <summary>
    /// 批量写入中的每一项，表示一条单独的 PLC 写入指令。
    /// 例如：写入某个 DB 地址、指定值类型以及要写入的具体数值。
    /// </summary>
    public List<PlcWriteRequest> Items { get; set; } = new();
}
