using Dapper;
using plc.Database;
using plc.Models;

namespace plc.Mappers;

/// <summary>
/// ConfigInfo 数据访问器。
/// 直接使用 Dapper 执行 SQL，并将结果映射为 ConfigInfo 对象。
/// </summary>
public class ConfigInfoMapper : IConfigInfoMapper
{
    private readonly MySqlOptions _options;

    public ConfigInfoMapper(MySqlOptions options)
    {
        _options = options;
    }

    public async Task<ConfigInfo?> SelectByIdAsync(int id)
    {
        await using var connection = await MySqlDatabase.OpenAsync(_options);
        const string sql = @"SELECT id, config
                             FROM mk_plc.plc_config_info
                             WHERE id = @Id";

        return await connection.QuerySingleOrDefaultAsync<ConfigInfo>(sql, new { Id = id });
    }
}
