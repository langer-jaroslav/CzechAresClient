using CzechAresClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Services;

public class Worker : BackgroundService
{
    private readonly ICzechAresClientService _aresClient;

    private readonly ILogger<Worker> _logger;

    public Worker(ICzechAresClientService aresClient, ILogger<Worker> logger)
    {
        _aresClient = aresClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        try
        {
            var aresResult = await _aresClient.SearchByCompanyIdAsync("27074358");

            _logger.LogInformation("found: " + aresResult);

            aresResult = await _aresClient.SearchByCompanyIdAsync("06279996");

            _logger.LogInformation("found: " + aresResult);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }

    }
}