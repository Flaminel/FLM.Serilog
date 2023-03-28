# Usage

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