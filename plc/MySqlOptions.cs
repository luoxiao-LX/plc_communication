namespace plc;

/// <summary>
/// MySQL 数据库连接配置项。
/// 这些参数通常来自 appsettings.json 或环境变量，用于拼接连接字符串。
/// </summary>
public class MySqlOptions
{
    public string Host { get; set; } = "localhost";
    public string Database { get; set; } = "mk_plc";
    public string User { get; set; } = "root";
    public string Password { get; set; } = "Lx906547535";
    public int Port { get; set; } = 3306;
}
