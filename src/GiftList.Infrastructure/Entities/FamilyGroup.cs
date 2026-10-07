namespace GiftList.Infrastructure.Entities;

public class FamilyGroup
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string InviteCode { get; set; }

    public ICollection<ApplicationUser> Members { get; set; } = [];
}
