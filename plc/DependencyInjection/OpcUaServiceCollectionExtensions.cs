using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using plc.OpcUa;

namespace plc.DependencyInjection;

public static class OpcUaServiceCollectionExtensions
{
    public static IServiceCollection AddOpcUaServices(this IServiceCollection services, IConfiguration configuration)
    {
        var opcUaOptions = configuration.GetSection("OpcUa").Get<OpcUaOptions>() ?? new OpcUaOptions();
        services.AddSingleton(opcUaOptions);
        return services;
    }
}
