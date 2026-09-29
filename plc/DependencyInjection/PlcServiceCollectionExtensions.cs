
using plc.Plc;
using plc.S7;
using S7.Net;

namespace plc.DependencyInjection;

public static class PlcServiceCollectionExtensions
{
    public static IServiceCollection AddPlcServices(this IServiceCollection services, IConfiguration configuration)
    {
        var s7Options = configuration.GetSection("S7").Get<S7Options>() ?? new S7Options();

        services.AddSingleton(s7Options);
        services.AddSingleton(new S7Client(
            s7Options.Ip,
            s7Options.Rack,
            s7Options.Slot,
            ParseCpuType(s7Options.CpuType),
            s7Options.Port));
        services.AddScoped<IPlcClient, PlcClient>();
        services.AddScoped<IPlcService, PlcService>();

        return services;
    }

    private static CpuType ParseCpuType(string cpuType)
    {
        return Enum.TryParse<CpuType>(cpuType, true, out var result)
            ? result
            : CpuType.S71200;
    }
}
