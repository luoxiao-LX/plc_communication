using plc.Models;

namespace plc.Mappers;

/// <summary>
/// 数据库映射层接口。
/// 定义针对配置表的查询方法，保持 Controller/Service 与数据访问层解耦。
/// </summary>
public interface IConfigInfoMapper
{
    Task<ConfigInfo?> SelectByIdAsync(int id);
}
