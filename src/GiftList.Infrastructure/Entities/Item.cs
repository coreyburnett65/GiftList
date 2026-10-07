namespace GiftList.Infrastructure.Entities;

public enum ItemPriority
{
    Low,
    Medium,
    High
}

public enum ItemStatus
{
    Active,
    Received
}

public class Item
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public ApplicationUser? Owner { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Url { get; set; }

    public decimal? Price { get; set; }

    public ItemPriority Priority { get; set; } = ItemPriority.Medium;

    public string? ImageUrl { get; set; }

    public string? Notes { get; set; }

    public int SortOrder { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.Active;

    public DateTimeOffset? ReceivedAt { get; set; }
}
