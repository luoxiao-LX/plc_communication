using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using plc.Plc;

namespace plc.Controllers;

[ApiController]
[Route("api/plc")]
public class PlcController : ControllerBase
{
    private readonly IPlcService _plcService;

    public PlcController(IPlcService plcService)
    {
        _plcService = plcService;
    }

    [HttpGet("read")]
    public async Task<ActionResult<ApiResult<object>>> Read([FromQuery] string node)
    {
        if (string.IsNullOrWhiteSpace(node))
        {
            return BadRequest(ApiResult<object>.Fail("node is required."));
        }

        try
        {
            await _plcService.ConnectAsync();
            var value = await _plcService.ReadTagAsync<object>(node);
            return Ok(ApiResult<object>.Ok(value, $"Read node {node} successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }

    [HttpGet("batch")]
    public async Task<ActionResult<ApiResult<object>>> ReadBatch([FromQuery] string[] nodes)
    {
        if (nodes == null || nodes.Length == 0 || nodes.All(string.IsNullOrWhiteSpace))
        {
            return BadRequest(ApiResult<object>.Fail("nodes is required."));
        }

        try
        {
            await _plcService.ConnectAsync();
            var values = await _plcService.ReadBatchAsync(nodes.Where(n => !string.IsNullOrWhiteSpace(n)));
            return Ok(ApiResult<object>.Ok(values, "Read batch nodes successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// 批量写入多个 PLC 地址。
    /// 该接口用于一次提交多个写入命令，适用于需要同时更新多个寄存器或位变量的场景。
    /// 请求体是一个标准的 <see cref="BatchWriteRequest"/>，内部包含多个 <see cref="PlcWriteRequest"/>。
    /// </summary>
    [HttpPost("batch-write")]
    public async Task<ActionResult<ApiResult<object>>> BatchWrite([FromBody] BatchWriteRequest request)
    {
        if (request == null || request.Items == null || request.Items.Count == 0)
        {
            return BadRequest(ApiResult<object>.Fail("request body or items is required."));
        }

        try
        {
            // 每一项都是一次独立的 PLC 写入，逐个处理并收集结果，确保任意一项失败也不会导致整个批次完全丢失。
            var results = new List<object>();

            foreach (var item in request.Items)
            {
                if (string.IsNullOrWhiteSpace(item.NodeId))
                {
                    results.Add(new { error = "nodeId is required." });
                    continue;
                }

                if (item.Value.ValueKind == JsonValueKind.Undefined || item.Value.ValueKind == JsonValueKind.Null)
                {
                    results.Add(new { nodeId = item.NodeId, error = "value is required." });
                    continue;
                }

                var valueType = item.ValueType.Trim();
                object parsedValue = ParseValue(item.Value, valueType);

                switch (valueType.ToLowerInvariant())
                {
                    case "short":
                        await _plcService.WriteTagAsync(item.NodeId, (short)parsedValue);
                        break;
                    case "int":
                        await _plcService.WriteTagAsync(item.NodeId, (int)parsedValue);
                        break;
                    case "long":
                        await _plcService.WriteTagAsync(item.NodeId, (long)parsedValue);
                        break;
                    case "float":
                        await _plcService.WriteTagAsync(item.NodeId, (float)parsedValue);
                        break;
                    case "double":
                        await _plcService.WriteTagAsync(item.NodeId, (double)parsedValue);
                        break;
                    case "bool":
                        await _plcService.WriteTagAsync(item.NodeId, (bool)parsedValue);
                        break;
                    case "string":
                        await _plcService.WriteTagAsync(item.NodeId, parsedValue.ToString()!);
                        break;
                    default:
                        results.Add(new { nodeId = item.NodeId, error = $"unsupported value type: {valueType}" });
                        continue;
                }

                results.Add(new { nodeId = item.NodeId, value = parsedValue });
            }

            return Ok(ApiResult<object>.Ok(results, $"Batch write completed. {results.Count} items processed."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// 连续读取 PLC 数据块中的一段原始字节。
    /// 适用于需要读取整块内存并在业务层自行解析为 short/int/float 的场景。
    /// 例如：读取 DB1 从偏移 0 开始，长度 16 字节的内容，再使用 <see cref="PlcByteParser"/> 解析。
    /// </summary>
    [HttpPost("read-block")]
    public async Task<ActionResult<ApiResult<object>>> ReadBlock([FromBody] PlcReadBlockRequest request)
    {
        if (request == null)
        {
            return BadRequest(ApiResult<object>.Fail("request body is required."));
        }

        if (request.DbNumber <= 0)
        {
            return BadRequest(ApiResult<object>.Fail("dbNumber must be greater than zero."));
        }

        if (request.StartByte < 0)
        {
            return BadRequest(ApiResult<object>.Fail("startByte must be greater than or equal to zero."));
        }

        if (request.Length <= 0)
        {
            return BadRequest(ApiResult<object>.Fail("length must be greater than zero."));
        }

        try
        {
            await _plcService.ConnectAsync();
            var bytes = await _plcService.ReadBlockAsync(request.DbNumber, request.StartByte, request.Length);

            // 返回原始字节数组 + 十六进制字符串，方便调试。
            return Ok(ApiResult<object>.Ok(new
            {
                dbNumber = request.DbNumber,
                startByte = request.StartByte,
                length = request.Length,
                bytes = bytes,
                hex = BitConverter.ToString(bytes).Replace("-", " ")
            }, "Read block successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }

    [HttpPost("write")]
    public async Task<ActionResult<ApiResult<object>>> Write([FromBody] PlcWriteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NodeId))
        {
            return BadRequest(ApiResult<object>.Fail("nodeId is required."));
        }

        if (request.Value.ValueKind == JsonValueKind.Undefined || request.Value.ValueKind == JsonValueKind.Null)
        {
            return BadRequest(ApiResult<object>.Fail("value is required."));
        }

        try
        {
            await _plcService.ConnectAsync();

            var valueType = request.ValueType.Trim();
            object parsedValue = ParseValue(request.Value, valueType);

            switch (valueType.ToLowerInvariant())
            {
                case "short":
                    await _plcService.WriteTagAsync(request.NodeId, (short)parsedValue);
                    break;
                case "int":
                    await _plcService.WriteTagAsync(request.NodeId, (int)parsedValue);
                    break;
                case "long":
                    await _plcService.WriteTagAsync(request.NodeId, (long)parsedValue);
                    break;
                case "float":
                    await _plcService.WriteTagAsync(request.NodeId, (float)parsedValue);
                    break;
                case "double":
                    await _plcService.WriteTagAsync(request.NodeId, (double)parsedValue);
                    break;
                case "bool":
                    await _plcService.WriteTagAsync(request.NodeId, (bool)parsedValue);
                    break;
                case "string":
                    await _plcService.WriteTagAsync(request.NodeId, parsedValue.ToString()!);
                    break;
                default:
                    return BadRequest(ApiResult<object>.Fail($"unsupported value type: {valueType}"));
            }

            return Ok(ApiResult<object>.Ok(new { nodeId = request.NodeId, value = parsedValue }, $"Write node {request.NodeId} successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }

    private static object ParseValue(JsonElement element, string valueType)
    {
        var type = valueType.Trim();

        return type.ToLowerInvariant() switch
        {
            "short" when element.ValueKind == JsonValueKind.String => short.Parse(element.GetString()!),
            "short" => element.GetInt16(),
            "int" when element.ValueKind == JsonValueKind.String => int.Parse(element.GetString()!),
            "int" => element.GetInt32(),
            "long" when element.ValueKind == JsonValueKind.String => long.Parse(element.GetString()!),
            "long" => element.GetInt64(),
            "float" when element.ValueKind == JsonValueKind.String => float.Parse(element.GetString()!),
            "float" => element.GetSingle(),
            "double" when element.ValueKind == JsonValueKind.String => double.Parse(element.GetString()!),
            "double" => element.GetDouble(),
            "bool" when element.ValueKind == JsonValueKind.String => bool.Parse(element.GetString()!),
            "bool" => element.GetBoolean(),
            "string" => element.GetString() ?? string.Empty,
            _ => throw new InvalidOperationException($"Unsupported value type: {type}")
        };
    }
}
