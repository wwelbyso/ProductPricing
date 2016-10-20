using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductPricing.Models
{
    public partial class ABHIPricingDBContext : DbContext
    {
        public ABHIPricingDBContext(DbContextOptions<ABHIPricingDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyComposition>(entity =>
            {
                entity.Property(e => e.FamilyCompositionId)
                    .HasColumnName("familyCompositionId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<Premium>(entity =>
            {
                entity.HasIndex(e => e.SumDeductId)
                    .HasName("IX_SumDeduct");

                entity.Property(e => e.PremiumId)
                    .HasColumnName("premiumID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Premium1)
                    .HasColumnName("Premium")
                    .HasColumnType("money");

                entity.Property(e => e.SumDeductId).HasColumnName("sumDeductID");

                entity.HasOne(d => d.SumDeduct)
                    .WithMany(p => p.Premium)
                    .HasForeignKey(d => d.SumDeductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Premium");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId)
                    .HasColumnName("productID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Condition)
                    .IsRequired()
                    .HasColumnName("condition")
                    .HasColumnType("char(20)");

                entity.Property(e => e.PlanName)
                    .IsRequired()
                    .HasColumnName("planName")
                    .HasColumnType("char(20)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("productName")
                    .HasColumnType("char(20)");

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasColumnName("productType")
                    .HasColumnType("char(20)");

                entity.HasOne(d => d.FamilyComposition)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FamilyCompositionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Family_Composition");
            });

            modelBuilder.Entity<SumDeduct>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_SumDeduct_ProductID");

                entity.Property(e => e.SumDeductId)
                    .HasColumnName("sumDeductID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Deductible).HasColumnName("deductible");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.SumInsured).HasColumnName("sumInsured");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SumDeduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_SumDeduct");
            });
        }

        public virtual DbSet<FamilyComposition> FamilyComposition { get; set; }
        public virtual DbSet<Premium> Premium { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<SumDeduct> SumDeduct { get; set; }
    }
}