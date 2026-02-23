using Microsoft.EntityFrameworkCore;
using Acts.Api.Models;

namespace Acts.Api.Data
{
    public class ActsDbContext : DbContext
    {
        public ActsDbContext(DbContextOptions<ActsDbContext> options)
            : base(options) { }

        public DbSet<Person> Person => Set<Person>();
        public DbSet<ExternalDuty> ExternalDuty => Set<ExternalDuty>();
        public DbSet<AstronautDuty> AstronautDuty => Set<AstronautDuty>();
        public DbSet<AstronautDetail> AstronautDetail => Set<AstronautDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.PersonId);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(p => p.Name)
                    .IsUnique();
            });

           modelBuilder.Entity<ExternalDuty>(entity =>
            {
                entity.HasKey(d => d.DutyId);

                entity.Property(d => d.Rank)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.ExternalDuties)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AstronautDuty>(entity =>
            {
                entity.HasKey(d => d.DutyId);

                entity.Property(d => d.Rank)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.AstronautDuties)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(d => d.PersonId)
                    .IsUnique()
                    .HasFilter("EndDate IS NULL");
            });

            modelBuilder.Entity<AstronautDetail>(entity =>
            {
                entity.HasKey(ad => ad.DutyId);

                entity.HasOne(ad => ad.AstronautDuty)
                    .WithOne(d => d.AstronautDetail)
                    .HasForeignKey<AstronautDetail>(ad => ad.DutyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}