using plc.Mappers;
using plc.Models;

namespace plc.Services;

/// <summary>
/// 配置查询服务实现类。
/// 负责协调 Mapper，将数据访问结果返回给 Controller 层。
/// </summary>
public class ConfigInfoServiceImpl : IUserInfoService
{
    private readonly IConfigInfoMapper _mapper;

    public ConfigInfoServiceImpl(IConfigInfoMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ConfigInfo?> GetByIdAsync(int id)
    {
        return await _mapper.SelectByIdAsync(id);
    }
}
