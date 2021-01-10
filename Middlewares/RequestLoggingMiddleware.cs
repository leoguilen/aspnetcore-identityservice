using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationClientService.Middlewares
{
    public class RequestLoggingMiddleware
    {
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private readonly Stopwatch _timer;
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _timer = new Stopwatch();
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _timer.Start();
                await _next(context);
                _timer.Stop();
            }
            finally
            {
                if (_timer.ElapsedMilliseconds > 500)
                {
                    _logger.LogWarning($"Long Running {MessageTemplate}",
                        context.Request?.Method,
                        context.Request?.Path,
                        context.Response?.StatusCode,
                        _timer.Elapsed.TotalMilliseconds);
                }

                _logger.LogInformation(MessageTemplate,
                    context.Request?.Method,
                    context.Request?.Path,
                    context.Response?.StatusCode,
                    _timer.Elapsed.TotalMilliseconds);
            }
        }
    }
}
