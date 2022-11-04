using Serilog;

namespace LoggerTest;

public class DsLoggerBaseConfiguration
{
  private readonly Task<string> _valuesTask;

  public DsLoggerBaseConfiguration()
  {
    _valuesTask = GetLoggerConfiguration();
  }

  private static async Task<string> GetLoggerConfiguration()
  {
    await Task.Delay(5000);
    return "ItWorks!";
  }

  public async Task<LoggerConfiguration> GetBaseLoggerConfiguration()
  {
    var result = await _valuesTask;
    var loggerConfiguration = new LoggerConfiguration()
      .Enrich.WithProperty("SourceContext", string.Empty)
      .Enrich.WithProperty("DoesItWork", result)
      .Enrich.FromLogContext()
      .WriteTo.Async(sinkConfiguration =>
        sinkConfiguration.Console(outputTemplate:
          "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceContext}) ({UserId}) ({DoesItWork}) {Message:lj}{NewLine}{Exception}"));
    return loggerConfiguration;
  }
}