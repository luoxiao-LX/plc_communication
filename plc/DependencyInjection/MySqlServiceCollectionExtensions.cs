using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using plc.Mappers;
using plc.Services;

namespace plc.DependencyInjection;

public static class MySqlServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mysqlOptions = configuration.GetSection("MySql").Get<MySqlOptions>() ?? new MySqlOptions();
        services.AddSingleton(mysqlOptions);
        services.AddScoped<IConfigInfoMapper, ConfigInfoMapper>();
        services.AddScoped<IUserInfoService, ConfigInfoServiceImpl>();
        return services;
    }
}
