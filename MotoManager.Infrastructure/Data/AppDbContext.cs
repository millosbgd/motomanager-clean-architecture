using Microsoft.EntityFrameworkCore;
using MotoManager.Domain.Entities;

namespace MotoManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
    public DbSet<ServiceOrderLabor> ServiceOrderLabors => Set<ServiceOrderLabor>();
    public DbSet<ServiceOrderMaterial> ServiceOrderMaterials => Set<ServiceOrderMaterial>();
    public DbSet<Material> Materials => Set<Material>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicles");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Model)
                  .HasMaxLength(100)
                  .IsRequired();
            entity.Property(v => v.Plate)
                  .HasMaxLength(20)
                  .IsRequired();
            entity.Property(v => v.ClientId)
                  .IsRequired();
            entity.HasOne(v => v.Client)
                  .WithMany()
                  .HasForeignKey(v => v.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Naziv)
                  .HasMaxLength(200)
                  .IsRequired();
            entity.Property(c => c.Adresa)
                  .HasMaxLength(200)
                  .IsRequired();
            entity.Property(c => c.Grad)
                  .HasMaxLength(100)
                  .IsRequired();
            entity.Property(c => c.PIB)
                  .HasMaxLength(20);
            entity.Property(c => c.Telefon)
                  .HasMaxLength(50)
                  .IsRequired();
            entity.Property(c => c.Email)
                  .HasMaxLength(100)
                  .IsRequired();

        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.ToTable("ServiceOrders");
            entity.HasKey(so => so.Id);
            entity.Property(so => so.BrojNaloga)
                  .HasMaxLength(50)
                  .IsRequired();
            entity.Property(so => so.Datum)
                  .IsRequired();
            entity.Property(so => so.OpisRada)
                  .HasMaxLength(1000)
                  .IsRequired();
            entity.Property(so => so.Kilometraza)
                  .IsRequired();

            // Foreign keys
            entity.HasOne(so => so.Client)
                  .WithMany()
                  .HasForeignKey(so => so.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(so => so.Vehicle)
                  .WithMany()
                  .HasForeignKey(so => so.VehicleId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Collections
            entity.HasMany(so => so.Labors)
                  .WithOne(l => l.ServiceOrder)
                  .HasForeignKey(l => l.ServiceOrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(so => so.Materials)
                  .WithOne(m => m.ServiceOrder)
                  .HasForeignKey(m => m.ServiceOrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ServiceOrderLabor>(entity =>
        {
            entity.ToTable("ServiceOrderLabors");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.OpisRadova)
                  .HasMaxLength(500)
                  .IsRequired();
            entity.Property(l => l.UkupnoVreme)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(l => l.Cena)
                  .HasPrecision(18, 2)
                  .IsRequired();
        });

        modelBuilder.Entity<ServiceOrderMaterial>(entity =>
        {
            entity.ToTable("ServiceOrderMaterials");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.MaterialId)
                  .IsRequired();
            entity.Property(m => m.Kolicina)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(m => m.JedinicnaCena)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(m => m.UkupnaCena)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.HasOne(m => m.Material)
                  .WithMany()
                  .HasForeignKey(m => m.MaterialId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Materials");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Naziv)
                  .HasMaxLength(200)
                  .IsRequired();
            entity.Property(m => m.JedinicnaCena)
                  .HasPrecision(18, 2)
                  .IsRequired();
        });
        });
    }
}
