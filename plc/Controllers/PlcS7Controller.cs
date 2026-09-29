using Microsoft.AspNetCore.Mvc;
using plc.Plc;
using plc.S7;

namespace plc.Controllers;

[ApiController]
[Route("api/test/plc-s7")]
public class PlcS7Controller : ControllerBase
{
    private readonly IPlcService _plcService;
    private readonly S7Options _options;

    public PlcS7Controller(IPlcService plcService, S7Options options)
    {
        _plcService = plcService;
        _options = options;
    }

    [HttpGet("health")]
    public async Task<ActionResult<ApiResult<object>>> Health()
    {
        try
        {
            await _plcService.ConnectAsync();

            var dbInt = await _plcService.ReadTagAsync<int>(S7AddressTemplates.DbInt);
            var dbReal = await _plcService.ReadTagAsync<float>(S7AddressTemplates.DbReal);
            var dbBool = await _plcService.ReadTagAsync<bool>(S7AddressTemplates.DbBool);

            return Ok(ApiResult<object>.Ok(new
            {
                ip = _options.Ip,
                port = _options.Port,
                rack = _options.Rack,
                slot = _options.Slot,
                cpuType = _options.CpuType,
                dbInt = dbInt,
                dbBool = dbBool,
                dbReal = dbReal,
                isConnected = _plcService.IsConnected
            }, "S7 PLC test connection succeeded."));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult<object>.Fail(ex.Message));
        }
    }
}
