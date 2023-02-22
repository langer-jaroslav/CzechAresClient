using System.Diagnostics;
using System.Globalization;
using CzechAresClient;
using Demo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;

var pathToExe = Process.GetCurrentProcess().MainModule!.FileName;
var pathToContentRoot = Path.GetDirectoryName(pathToExe)!;
Directory.SetCurrentDirectory(pathToContentRoot);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<ICzechAresClientService, CzechAresClientService>();

        services.AddHostedService<Worker>();

        services.AddHttpClient(); // coz calling web apis
    })
    .Build();

await host.RunAsync();