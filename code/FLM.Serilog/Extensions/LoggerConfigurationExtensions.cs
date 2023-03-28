using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Templates;

namespace FLM.Serilog.Extensions;

public static class LoggerConfigurationExtensions
{
    private const string JsonTemplate = "{ { Timestamp: UtcDateTime(@t), Message: @m, MessageTemplate: if @mt <> @m then @mt else undefined(), Properties: @p } }\n";
    private const string DevTemplate = "[{@t:HH:mm:ss.fff} {@l:u3}]{#if SourceContext is not null} ({SourceContext}){#end} {@m}\n{@x}";

    public static LoggerConfiguration ApplyDefaults(
        this LoggerConfiguration loggerConfiguration,
        bool isDevelopmentEnv,
        bool useRequestLogging)
    {
        loggerConfiguration
            .ApplyDefaultSinks(isDevelopmentEnv)
            .ApplyDefaultEnrich()
            .ApplyDefaultOverrides(isDevelopmentEnv, useRequestLogging);

        return loggerConfiguration;
    }

    public static LoggerConfiguration ApplyDefaultSinks(this LoggerConfiguration loggerConfiguration, bool isDevelopmentEnv)
    {
        loggerConfiguration
            .WriteTo.Async(
                configure: sinkConfiguration => sinkConfiguration.Console(
                    new ExpressionTemplate(isDevelopmentEnv ? DevTemplate : JsonTemplate)),
                blockWhenFull: true);

        return loggerConfiguration;
    }

    public static LoggerConfiguration ApplyDefaultEnrich(this LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithExceptionDetails()
            .Enrich.WithSpan();

        return loggerConfiguration;
    }

    public static LoggerConfiguration ApplyDefaultOverrides(
        this LoggerConfiguration loggerConfiguration,
        bool isDevelopmentEnv,
        bool useRequestLogging)
    {
        var logLevelOverrides = GetDefaultLogLevelOverrides(isDevelopmentEnv, useRequestLogging);

        foreach (KeyValuePair<string, LogEventLevel> pair in logLevelOverrides)
        {
            loggerConfiguration.MinimumLevel.Override(pair.Key, pair.Value);
        }

        return loggerConfiguration;
    }

    private static Dictionary<string, LogEventLevel> GetDefaultLogLevelOverrides(bool isDevelopmentEnv, bool useRequestLogging)
    {
        Dictionary<string, LogEventLevel> logLevelOverrides = new()
        {
            { "Microsoft.EntityFrameworkCore", LogEventLevel.Warning },
            { "Quartz", LogEventLevel.Warning }
        };

        if (useRequestLogging)
        {
            logLevelOverrides.Add("Microsoft.AspNetCore", LogEventLevel.Warning);
        }

        if (isDevelopmentEnv)
        {
            return logLevelOverrides;
        }

        logLevelOverrides.Add("System", LogEventLevel.Warning);
        logLevelOverrides.Add("Microsoft", LogEventLevel.Warning);
        logLevelOverrides.Add("Microsoft.Hosting.Lifetime", LogEventLevel.Information);

        return logLevelOverrides;
    }
}