using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using plc.OpcUa;
using plc.Plc;

namespace plc.Controllers;

[ApiController]
[Route("api/test/upcua")]
[Route("api/test/opcua")]
public class UpcUaController : ControllerBase
{
    private readonly OpcUaOptions _options;

    public UpcUaController(OpcUaOptions options)
    {
        _options = options;
    }

    [HttpGet("health")]
    public ActionResult<ApiResult<object>> Health()
    {
        return Ok(ApiResult<object>.Ok(new
        {
            serverUrl = _options.ServerUrl,
            sessionTimeoutMs = _options.SessionTimeoutMs,
            useSecurity = _options.UseSecurity,
            defaultNodePrefix = _options.DefaultNodePrefix,
            note = "This is an OPC UA test interface. Actual transport implementation can be injected later by the client layer."
        }, "OPC UA test interface is ready."));
    }

    [HttpPost("connect")]
    public async Task<ActionResult<ApiResult<object>>> Connect()
    {
        await Task.CompletedTask;

        return Ok(ApiResult<object>.Ok(new
        {
            serverUrl = _options.ServerUrl,
            isConnected = false,
            note = "This is a placeholder connection check. Implement the actual OPC UA client connection in the dedicated client layer."
        }, "OPC UA connection test request accepted."));
    }
}
