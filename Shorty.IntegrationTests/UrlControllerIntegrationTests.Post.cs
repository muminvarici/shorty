using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shorty.Api.Domain.Constants;
using Shorty.Api.Infrastructure.Data;
using Shorty.Api.Presentation.Contracts.Requests;
using Shorty.IntegrationTests.Helpers.Builders;

namespace Shorty.IntegrationTests;

public partial class UrlControllerIntegrationTests : TestBase, IAsyncDisposable
{
    private readonly UrlBuilder _builder;
    private string _createdCode;

    public UrlControllerIntegrationTests()
    {
        _builder = new UrlBuilder(Factory);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(30)]
    public async Task Post_Should_Not_Allow_NonAllowed_Code_Length(int codeLength)
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com",
            CodeLength = codeLength
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Should().BeNull();

        #endregion Assert
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Post_Should_Not_Allow_Exceeded_Length_Characters(bool allow)
    {
        #region Arrange

        var length = allow
            ? UrlConstants.MaxUrlLength
            : UrlConstants.MaxUrlLength + 1;

        var request = new ShortUrlRequest
        {
            Url = "https://example.com/".PadRight(length, 'x')
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);

        #endregion Act

        #region Assert

        if (!allow)
        {
            statusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().BeNull();
        }
        else
        {
            SetCode(response.ShortUrl);
            statusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();
            response!.LongUrl.Should().Be(request.Url);
        }

        #endregion Assert
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\n")]
    [InlineData("\t")]
    public async Task Post_Should_Not_Allow_Null_Or_Empty_Url(string url)
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = url,
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Should().BeNull();

        #endregion Assert
    }

    [Theory]
    [InlineData(10)]
    [InlineData(21)]
    public async Task Post_Should_Return_Specified_Code_Length(int codeLength)
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com",
            CodeLength = codeLength,
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);
        SetCode(response.ShortUrl);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull(); 
        _createdCode.Length.Should().Be(codeLength);

        #endregion Assert
    }

    private void SetCode(string url)
    {
        _createdCode = url[(url.LastIndexOf('/') + 1)..];
    }

    [Theory]
    [InlineData(10)]
    [InlineData(21)]
    public async Task Post_Should_Return_Specified_End_Date(int daysLater)
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com",
            LastUsageDate = DateTime.Today.AddDays(daysLater)
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);
        SetCode(response.ShortUrl);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.ValidUntil.Should().Be(DateTime.Today.AddDays(daysLater));

        #endregion Assert
    }

    [Fact]
    public async Task Post_Should_Not_Allow_Date_Before_Now()
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com",
            LastUsageDate = DateTime.Now.AddMinutes(-1)
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Should().BeNull();

        #endregion Assert
    }

    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Post_Should_Set_IsSingleUsage(bool? isSingleUsage)
    {
        #region Arrange

        var request = new ShortUrlRequest
        {
            Url = "https://example.com",
            IsSingleUsage = isSingleUsage
        };

        #endregion Arrange

        #region Act

        var (statusCode, response) = await _builder.ShortenUrl(request);
        SetCode(response.ShortUrl);

        #endregion Act

        #region Assert

        statusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.IsSingleUsage.Should().Be(isSingleUsage ?? false);
        response.LongUrl.Should().Be(request.Url);

        #endregion Assert
    }


    public async ValueTask DisposeAsync()
    {
        var dbContext = Scope.ServiceProvider.GetRequiredService(typeof(ApplicationDbContext)) as ApplicationDbContext;
        var entity = await dbContext.UrlDetails.SingleAsync(w => w.Code == _createdCode);
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();

        base.Scope.Dispose();
    }
}