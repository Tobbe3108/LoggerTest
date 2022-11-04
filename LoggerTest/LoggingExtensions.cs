using Autofac;

namespace LoggerTest;

public static class LoggingExtensions
{
  public static void RegisterLogging(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterType<DsLoggerBaseConfiguration>()
      .As<DsLoggerBaseConfiguration>()
      .SingleInstance();

    containerBuilder.RegisterType<DsLogger>()
      .As<DsLogger>()
      .InstancePerLifetimeScope();

    containerBuilder.Register(c => c.Resolve<DsLogger>().LoggerFactoryTask().GetAwaiter().GetResult())
      .As<ILoggerFactory>()
      .InstancePerLifetimeScope();

    containerBuilder.RegisterGeneric(typeof(Logger<>))
      .As(typeof(ILogger<>))
      .InstancePerLifetimeScope();
  }
}