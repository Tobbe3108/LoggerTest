using Autofac;
using Autofac.Extensions.DependencyInjection;
using LoggerTest;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly).AsImplementedInterfaces();
  containerBuilder.RegisterLogging();
});

var app = builder.Build();

app.MapGet("/Logger1",
  async ([FromServices] ILogger<Program> logger,
    [FromServices] DsLogger customSerilogProvider) =>
  {
    await customSerilogProvider.UpdateParameters("Logger1");
    while (true)
    {
      logger.LogInformation("Logger1");
      await Task.Delay(1000);
    }
  });

app.MapGet("/Logger2",
  async ([FromServices] ILogger<Program> logger,
    [FromServices] DsLogger customSerilogProvider) =>
  {
    await customSerilogProvider.UpdateParameters("Logger2");
    while (true)
    {
      logger.LogInformation("Logger2");
      await Task.Delay(1000);
    }
  });

await app.RunAsync();