using fareShare.Models;
using Microsoft.EntityFrameworkCore;

namespace fareShare.Migrations;

public class BillDbContext : DbContext
{
    public DbSet<Bill> Bill { get; set; }

    public DbSet<User> Users { get; set; }

    public BillDbContext(DbContextOptions<BillDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        {
            // Zian -> I added the entity configuration for the Bill class.
            // The Bill link class is a one to many relationship, there is one bill to many BillLinks
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.BillId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
            });

            modelBuilder.Entity<BillLink>(entity =>
            {
                entity.HasKey(e => e.BillLinkId);
                entity
                    .HasOne(dl => dl.Bill)
                    .WithMany(b => b.BillLinks) // One Bill to many BillLinks
                    .HasForeignKey(dl => dl.BillId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete to remove related BillLinks when a Bill is deleted
            });

            // MAIA~ DataBase not yet intergrated. Info > Backend C# Lesson 11: Token Authentication > Create the User Data Model > https://bethel.populiweb.com/router/courseofferings/10739695/lessons/10916755/pages/12026113/show
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
            });
        }
    }
}
