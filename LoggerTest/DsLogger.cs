using Serilog;

namespace LoggerTest;

public class DsLogger : IAsyncDisposable
{
  private readonly DsLoggerBaseConfiguration _dsLoggerBaseConfiguration;
  private readonly Task<SwitchableLogger> _switchableLoggerTask;
  private string? _userId;

  public DsLogger(DsLoggerBaseConfiguration dsLoggerBaseConfiguration)
  {
    _dsLoggerBaseConfiguration = dsLoggerBaseConfiguration;
    _switchableLoggerTask = Task.Run(async () =>
      new SwitchableLogger((await _dsLoggerBaseConfiguration.GetBaseLoggerConfiguration()).CreateLogger()));
  }

  public async Task<ILoggerFactory> LoggerFactoryTask() => new LoggerFactory().AddSerilog(await _switchableLoggerTask, true);

  public async Task UpdateParameters(string userId)
  {
    _userId = userId;
    await UpdateLoggerConfiguration(await _dsLoggerBaseConfiguration.GetBaseLoggerConfiguration());
  }

  private async Task UpdateLoggerConfiguration(LoggerConfiguration loggerConfiguration)
  {
    if (_userId is not null) loggerConfiguration.Enrich.WithProperty("UserId", _userId);
    var newLogger = loggerConfiguration.CreateLogger();
    (await _switchableLoggerTask).Set(newLogger);
  }

  public async ValueTask DisposeAsync() => (await _switchableLoggerTask).Dispose();
}