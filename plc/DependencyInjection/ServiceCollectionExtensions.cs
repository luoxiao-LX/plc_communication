namespace plc.DependencyInjection;

/// <summary>
/// 总入口：按功能模块聚合各类依赖注入扩展。
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMySqlServices(configuration);
        services.AddOpcUaServices(configuration);
        services.AddPlcServices(configuration);
        return services;
    }
}
