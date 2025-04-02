using team.Models;
using Microsoft.EntityFrameworkCore;

namespace team.Migrations;

public class BillDbContext : DbContext
{
    //public DbSet<Bill> Bill { get; set; }

    public DbSet<User> Users { get; set; }

    public BillDbContext(DbContextOptions<BillDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Bill>(entity =>
        {
            // MAIA~ Im not sure what we want to call the properties for the bills yet so feel free to change below. I kept the example from Backend framworks LESSON 10: Rest APIs > Setting up data > https://bethel.populiweb.com/router/courseofferings/10739695/lessons/10916749/pages/12026035/show

            // entity.HasKey(e => e.BillId);
            // entity.Property(e => e.Name).IsRequired();
            // entity.Property(e => e.Description).IsRequired();
            // entity.Property(e => e.Price).IsRequired();


            // MAIA~ DataBase not yet intergrated. Info > Backend C# Lesson 11: Token Authentication > Create the User Data Model > https://bethel.populiweb.com/router/courseofferings/10739695/lessons/10916755/pages/12026113/show
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
            });
        }//);

        // modelBuilder
        //     .Entity<Bill>()
        //     .HasData(
        //         new Bill
        //         {
        //             BillId = 1,
        //             // Name = "Cappuccino",
        //             // Description = "Freshly pulled double espresso with steamed milk",
        //             // Price = 3.99,
        //         },
        //         new Bill
        //         {
        //             BillId = 2,
        //             // Name = "Americano",
        //             // Description = "Flavorful espresso topped with hot water",
        //             // Price = 2.49,
        //         }
        //     );
    }
}
