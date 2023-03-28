# Usage

## Bootstrap logger
```
Log.Logger = new LoggerConfiguration()
    .CreateDefaultBootstrapLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .Configure()
        .Build();

    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Logger.Fatal(exception, "Application failed to start");
}
finally
{
    await Log.CloseAndFlushAsync();
}
```

## Workers
```
IHost host = Host
    .CreateDefaultBuilder(args)
    .UseSerilog()
    .Build();
```

## APIs

### With request logging
```
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(useRequestLogging: true);

WebApplication app = builder.Build();
app.UseSerilogRequestLogging();
```

### Without request logging
```
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

WebApplication app = builder.Build();
```