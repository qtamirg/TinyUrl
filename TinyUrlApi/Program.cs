using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TinyUrlApi.Components;
using TinyUrlApi.DataModels;
using TinyUrlApi.Models;
using TinyUrlApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<ShortenedUrlConnectionParameters>(builder.Configuration.GetSection("ShortenedUrlConnectionParameters"));
builder.Services.AddSingleton<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddSingleton<IUrlRepository, UrlRepository>();
builder.Services.AddSingleton<IShortUrlGenerator, ShortUrlGenerator>();
builder.Services.AddSingleton<IMemoryCache, RedirectionsMemoryCache>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
