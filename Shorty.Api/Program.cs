using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorty.Api.Application.Services;
using Shorty.Api.Infrastructure.Data;
using Shorty.Api.Presentation.Contracts.Requests;
using Shorty.Api.Presentation.Contracts.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UrlService>()
    .AddScoped<CurrentUserService>();
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlite(connectionString); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1/url/{code}", async (UrlService urlService, [FromRoute] string code) =>
    {
        var longUrl = await urlService.GenerateLongUrl(code);
        if (longUrl == null) return Results.NotFound("Url not found!");
        return Results.Redirect(longUrl);
    })
    .WithDescription("Go to short URL")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status302Found)
    .WithOpenApi();

app.MapPost("/api/v1/url",
        async (UrlService urlService, [FromBody] ShortUrlRequest request) =>
        {
            var response = await urlService.GenerateShortUrl(request);
            if (response == null) return Results.BadRequest();

            return Results.Ok(response);
        })
    .WithDescription("Generate short URL")
    .Produces(StatusCodes.Status400BadRequest)
    .Produces<ShortUrlResponse>()
    .WithOpenApi();

app.Run();

/// <summary>
/// For testing only
/// </summary>
public sealed partial class Program;