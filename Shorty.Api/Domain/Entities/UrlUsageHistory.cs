﻿namespace Shorty.Api.Domain.Entities;

public class UrlUsageHistory : EntityBase
{
    public Guid Id { get; set; }
    public int UrlId { get; set; }

    public virtual UrlDetail? UrlDetail { get; set; }
}