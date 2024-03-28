using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shorty.Api.Domain.Constants;

namespace Shorty.Api.Presentation.Contracts.Requests;

public record ShortUrlRequest
{
    [Required]
    [Description("REQUIRED: Url that you want the short version")]
    [DefaultValue("example.com")]
    public string Url { get; set; } = string.Empty;

    [Range(UrlConstants.MinCodeLength, UrlConstants.MaxCodeLength)]
    [Description("OPTIONAL: Use this parameter if you want a specific code length. Default value is '7'")]
    [DefaultValue(null)]
    public int? CodeLength { get; set; }

    [Description("OPTIONAL: Use this value if you need to specify end date")]
    [DefaultValue(null)]
    public DateTime? LastUsageDate { get; set; }

    public bool? IsSingleUsage { get; set; }
}