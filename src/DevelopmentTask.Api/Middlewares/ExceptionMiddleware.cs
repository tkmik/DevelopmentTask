using System.Net.Mime;
using System.Text;
using System.Text.Json;
using DevelopmentTask.Api.Common;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Entities;

namespace DevelopmentTask.Api.Middlewares
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Guid correlationId = Guid.Empty;
            long eventId = 0;

            try
            {
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var exceptionJournalRepository = scope.ServiceProvider.GetRequiredService<IExceptionJournalRepository>();

                correlationId = Guid.TryParse(context.Items[ApiConsts.CorrelationIdHeaderName]?.ToString(), out Guid result)
                        ? result
                        : Guid.NewGuid();

                var queryParams = context.Request.Query
                    .ToDictionary(k => k.Key, v => v.Value);

                string? body = null;

                context.Request.EnableBuffering();

                using (var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                var bodyJson = GetBodyJson(body);

                var exceptionJournal = new ExceptionJournal
                {
                    EventId = exception.HResult,
                    CorrelationId = correlationId,
                    Timestamp = TimeProvider.System.TimestampFrequency,
                    RequestPath = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    ExceptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    QueryParams = JsonSerializer.Serialize(queryParams),
                    BodyParams = bodyJson
                };

                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                    await exceptionJournalRepository.AddAsync(exceptionJournal, cts.Token);

                    eventId = exceptionJournal.Id;
                }
                catch (OperationCanceledException oce)
                {
                    _logger.OperationCancelledError(oce);
                }
            }
            catch (Exception ex)
            {
                _logger.UnexpectedError(ex);
            }
            finally
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                await context.Response.WriteAsJsonAsync(new
                {
                    id = eventId,
                    correlationId = correlationId,
                    type = nameof(exception),
                    data = new
                    {
                        message = exception.Message
                    }
                });
            }
        }

        private string GetBodyJson(string body)
        {
            string bodyJson;

            if (string.IsNullOrWhiteSpace(body))
            {
                bodyJson = "{}";
            }
            else
            {
                try
                {
                    using var doc = JsonDocument.Parse(body);
                    bodyJson = body;
                }
                catch
                {
                    bodyJson = JsonSerializer.Serialize(new { raw = body });
                }
            }

            return bodyJson;
        }
    }
}
