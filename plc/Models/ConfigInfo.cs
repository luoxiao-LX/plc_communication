namespace plc.Models;

/// <summary>
/// 配置实体类。
/// 对应数据库中的配置表记录，用于演示从 MySQL 中读取配置数据。
/// </summary>
public class ConfigInfo
{
    public int Id { get; set; }
    public string Config { get; set; } = string.Empty;
}
