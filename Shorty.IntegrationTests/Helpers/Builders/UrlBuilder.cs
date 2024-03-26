using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Headers;
using Shorty.Api.Presentation.Contracts.Requests;
using Shorty.Api.Presentation.Contracts.Responses;

namespace Shorty.IntegrationTests.Helpers.Builders;

public class UrlBuilder(ApiFactory factory) : BuilderBase(factory)
{
    public async Task<(HttpStatusCode statusCode, ShortUrlResponse? response)> ShortenUrl(ShortUrlRequest payload)
    {
        var client = CreateClient();
        var request = new HttpRequestMessage
        {
            Content = new StringContent(Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json),
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/v1/url", UriKind.Relative)
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return (response.StatusCode, null);

        var contentResult = await response.Content.ReadAsStringAsync();
        var result = Deserialize<ShortUrlResponse>(contentResult);

        return (response.StatusCode, result);
    }

    public async Task<(HttpStatusCode StatusCode, string? location)> CallShortUrl(string code)
    {
        var client = CreateClient();
        var request = new HttpRequestMessage
        {
            Content = new StringContent(string.Empty),
            Method = HttpMethod.Get,
            RequestUri = new Uri($"/api/v1/url/{code}", UriKind.Relative)
        };

        var response = await client.SendAsync(request);
        var location = response.Headers.First(w => w.Key == "Location")
            .Value.SingleOrDefault();
        return (response.StatusCode, location);
    }

    private static string Serialize<T>(T payload)
    {
        return JsonSerializer.Serialize(payload);
    }

    private static T? Deserialize<T>(string content)
    {
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}

public abstract class BuilderBase
{
    protected ApiFactory Factory { get; }

    protected BuilderBase(ApiFactory factory)
    {
        Factory = factory;
    }

    protected HttpClient CreateClient()
    {
        return Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //if auth needed, configure it
                });
            })
            .CreateDefaultClient();
    }
}