using plc.Models;

namespace plc.Services;

/// <summary>
/// 配置查询服务接口。
/// 通过此接口暴露业务能力，隐藏底层 Mapper 实现细节。
/// </summary>
public interface IUserInfoService
{
    Task<ConfigInfo?> GetByIdAsync(int id);
}
