using GiftList.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftList.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<FamilyGroup> FamilyGroups => Set<FamilyGroup>();

    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(user =>
        {
            user.Property(u => u.Name).HasMaxLength(200).IsRequired();

            user.HasOne(u => u.FamilyGroup)
                .WithMany(g => g.Members)
                .HasForeignKey(u => u.FamilyGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<FamilyGroup>(group =>
        {
            group.Property(g => g.Name).HasMaxLength(200).IsRequired();
            group.Property(g => g.InviteCode).HasMaxLength(64).IsRequired();
            group.HasIndex(g => g.InviteCode).IsUnique();
        });

        builder.Entity<Item>(item =>
        {
            item.Property(i => i.Name).HasMaxLength(200).IsRequired();
            item.Property(i => i.Price).HasColumnType("decimal(10,2)");

            item.HasOne(i => i.Owner)
                .WithMany()
                .HasForeignKey(i => i.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
