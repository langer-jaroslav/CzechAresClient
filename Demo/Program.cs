using CzechAresClient;
using Demo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<ICzechAresClientService, CzechAresClientService>();

        services.AddHostedService<Worker>();

        services.AddHttpClient(); // coz calling web apis
    })
    .Build();

await host.RunAsync();