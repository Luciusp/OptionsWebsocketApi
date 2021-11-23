using OptionsWebsocketApi.Models.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (string.IsNullOrWhiteSpace(environment))
{
    environment = "development";
}

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(t => t.AddServerHeader = false);

// Add services to the container.
builder.Services.AddLogging(t =>
{
    t.ClearProviders();
    t.AddSerilog(dispose: true);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ConfigureLogging(environment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureLogging(string env)
{
    var doconsolelogging = Convert.ToBoolean(builder.Configuration[ConfigurationConstants.LoggingSerilogConsoleEnabled]);
    var excludeboilerplate = Convert.ToBoolean(builder.Configuration[ConfigurationConstants.LoggingSerilogExcludeBoilerplate]);

    var consoleminlevel = builder.Configuration[ConfigurationConstants.LoggingSerilogConsoleMinLevel];

    var servicename = builder.Configuration[ConfigurationConstants.ServiceName];

    var cfg = new LoggerConfiguration();
    cfg.Enrich.WithExceptionData()
        .Enrich.FromLogContext().MinimumLevel.Verbose();

    if (excludeboilerplate)
    {
        cfg.Filter.ByExcluding(Matching.FromSource("Microsoft"));
        cfg.Filter.ByExcluding(Matching.FromSource("IdentityServer4"));
        cfg.Filter.ByExcluding(Matching.FromSource("System.Net.Http.HttpClient"));
    }

    if (doconsolelogging)
    {
        LogEventLevel clevel = ParseMinLevel(consoleminlevel);
        cfg.WriteTo.Logger(config => config.MinimumLevel.Verbose().WriteTo.Console(clevel,
            "{Timestamp:HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}",
            theme: SystemConsoleTheme.Literate));
    }

    Log.Logger = cfg.CreateLogger();
    Log.Information($"{servicename} logging online with environment {env}");

}

static LogEventLevel ParseMinLevel(string minimum)
{
    LogEventLevel level = LogEventLevel.Debug;
    switch (minimum)
    {
        case "debug":
            level = LogEventLevel.Debug;
            break;
        case "verbose":
            level = LogEventLevel.Verbose;
            break;
        case "information":
            level = LogEventLevel.Information;
            break;
        case "warning":
            level = LogEventLevel.Warning;
            break;
        case "error":
            level = LogEventLevel.Error;
            break;
        case "fatal":
            level = LogEventLevel.Fatal;
            break;
    }

    return level;
}
