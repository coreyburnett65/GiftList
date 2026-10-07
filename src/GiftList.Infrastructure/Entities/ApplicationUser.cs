using Microsoft.AspNetCore.Identity;

namespace GiftList.Infrastructure.Entities;

public enum UserRole
{
    Member,
    Admin
}

/// <summary>
/// A managed child (see SPEC.md 5.7) is a row here with IsManaged = true and no usable
/// password set, which is why Identity (rather than an external IdP) owns this model.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public required string Name { get; set; }

    public Guid? FamilyGroupId { get; set; }

    public FamilyGroup? FamilyGroup { get; set; }

    public UserRole Role { get; set; } = UserRole.Member;

    public DateOnly? Birthday { get; set; }

    public bool IsManaged { get; set; }

    public bool ChildCanLogIn { get; set; }

    public bool ChildCanEdit { get; set; }
}
