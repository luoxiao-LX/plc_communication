namespace plc.S7;

/// <summary>
/// Siemens S7 地址模板。
/// 对应 S7.Net 中的字符串变量格式，例如 DB1.DBW0 表示数据块 1 中的字变量偏移 0。
/// </summary>
public static class S7AddressTemplates
{
    public const string DbInt = "DB1.DBW0";
    public const string DbReal = "DB1.DBD2";
    public const string DbBool = "DB1.DBX6.0";
}
