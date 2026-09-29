using MySqlConnector;

namespace plc.Database;

/// <summary>
/// MySQL 连接字符串构建器。
/// 集中管理数据库连接参数，避免在业务代码中硬编码连接串。
/// </summary>
public static class MySqlDatabase
{
    /// <summary>
    /// 根据配置项组装数据库连接字符串。
    /// </summary>
    public static string BuildConnectionString(MySqlOptions options)
    {
        var host = string.IsNullOrWhiteSpace(options.Host) ? "localhost" : options.Host;
        var database = string.IsNullOrWhiteSpace(options.Database) ? "mk_plc" : options.Database;
        var user = string.IsNullOrWhiteSpace(options.User) ? "root" : options.User;
        var password = options.Password ?? string.Empty;
        var port = options.Port > 0 ? options.Port : 3306;

        return $"Server={host};Database={database};Uid={user};Pwd={password};Port={port};AllowPublicKeyRetrieval=true;SslMode=None;Character Set=utf8;";
    }

    /// <summary>
    /// 创建并打开一个数据库连接对象。
    /// </summary>
    public static async Task<MySqlConnection> OpenAsync(MySqlOptions options)
    {
        var connection = new MySqlConnection(BuildConnectionString(options));
        await connection.OpenAsync();
        return connection;
    }
}
