using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShortenerUrl;
using ShortenerUrl.Entities;
using ShortenerUrl.ViewsModel;
using System.Diagnostics;
using ShortenerUrl.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ShortenerDb>(opt => opt.UseInMemoryDatabase("ShortenerUrl"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IValidator<UrlDTO>, ShortenerInputValidator>();
builder.Services.ExtensionCors();

var app = builder.Build();

app.UseCors("cors-policy");

app.MapGroup("/shortener")
            .ApiEndPoints()
            .WithTags("Url Shortener")
            .AddEndpointFilter(async (efiContext, next) =>
                                {
                                    var stopwatch = Stopwatch.StartNew();
                                    var result = await next(efiContext);
                                    stopwatch.Stop();
                                    var elapsed = stopwatch.ElapsedMilliseconds;
                                    var response = efiContext.HttpContext.Response;
                                    response.Headers.TryAdd("X-Response-Time", $"{elapsed} milliseconds");
                                    return result;
                                });
app.Run();
