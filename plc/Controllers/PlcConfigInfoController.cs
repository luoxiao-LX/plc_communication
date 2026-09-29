using Microsoft.AspNetCore.Mvc;
using plc.Models;
using plc.Services;

namespace plc.Controllers;

[ApiController]
[Route("api/config")]
public class PlcConfigInfoController : ControllerBase
{
    private readonly IUserInfoService _service;

    public PlcConfigInfoController(IUserInfoService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConfigInfo>> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
