using System.Net;
using FluentAssertions;
using Shorty.Api.Presentation.Contracts.Requests;

namespace Shorty.IntegrationTests;

public partial class UrlControllerIntegrationTests
{
    [Fact]
    public async Task Get_Should_Redirect_Related_Entry()
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com/",
        };
        var (isSuccess, record) = await _builder.ShortenUrl(request);
        isSuccess.Should().Be(HttpStatusCode.OK);

        SetCode(record.ShortUrl);

        #endregion Arrange

        #region Act

        var (statusCode, location) = await _builder.CallShortUrl(_createdCode);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.Found);
        location.Should().Be(request.Url);

        #endregion Assert
    }
}