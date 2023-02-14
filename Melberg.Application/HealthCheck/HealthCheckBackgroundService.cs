using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Health;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Melberg.Application.Health;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IHealthCheckChecker _checker;
    public HealthCheckBackgroundService(
        IHealthCheckChecker checker,
        ILogger logger)
    {
        _checker = checker;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        if(!HttpListener.IsSupported)
        {
            _logger.LogError("Oops");
        }
        _logger.LogInformation("Beginning Health Checker");
        try
        {
            using(var listener = new HttpListener())
            {

                listener.Prefixes.Add("http://+:8180/health/");
                listener.Start();
                while(!token.IsCancellationRequested)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest req = context.Request;


                    using HttpListenerResponse resp = context.Response;
                    string data = "Hello there!";
                    if(req.HttpMethod != WebRequestMethods.Http.Get || req.Url.PathAndQuery != "/health")
                    {
                        data = "oops";
                        resp.StatusCode = 404;
                    }
                    else
                    {

                        var status = await _checker.IsOk();

                        if(status)
                        {
                            data = "Health Check Succeeded";
                            resp.StatusCode = 200;
                            _logger.LogInformation(data);
                        }
                        else
                        {
                            data = "Health Check Failed";
                            resp.StatusCode = 500;
                            _logger.LogError(data);
                        }
                    }


                    resp.Headers.Set("Content-Type", "text/plain");

                    byte[] buffer = Encoding.UTF8.GetBytes(data);
                    resp.ContentLength64 = buffer.Length;

                    using Stream ros = resp.OutputStream;
                    ros.Write(buffer, 0, buffer.Length);
                }
                listener.Stop();
            };
        }
        catch (System.Exception ex)
        {
            
            throw;
        }

    }
}