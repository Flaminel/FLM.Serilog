using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FLM.Serilog.Extensions;

public static class HostingExtensions
{
    public static IHostBuilder UseSerilog(this IHostBuilder builder, bool useRequestLogging = false)
    {
        builder.UseSerilog((context, loggerConfiguration)
            => loggerConfiguration.ApplyDefaults(context.HostingEnvironment.IsDevelopment(), useRequestLogging));

        return builder;
    }

    public static IApplicationBuilder UseSerilogRequestLoggingF(this IApplicationBuilder builder)
    {
        builder.UseSerilogRequestLogging();

        return builder;
    }
}