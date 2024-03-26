namespace Shorty.Api.Domain.Entities;

public abstract class EntityBase
{
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}