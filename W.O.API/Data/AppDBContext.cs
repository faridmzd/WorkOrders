using Microsoft.EntityFrameworkCore;
using W.O.API.Domain;

namespace W.O.API.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasMany(e => e.Visits)
                .WithOne()
                .HasForeignKey(e => e.WorkOrderId)
                .HasConstraintName("FK_WORK_ORDER_ID")
                .IsRequired();

                entity.Property(e => e.Title).IsRequired().HasColumnType("nvarchar(50)").HasColumnName("TITLE");
                entity.Property(e => e.Description).IsRequired().HasColumnType("nvarchar(500)").HasColumnName("DESCRIPTION");
                entity.Property(e => e.Email).IsRequired().HasColumnType("nvarchar(50)").HasColumnName("EMAIL");
                entity.Property(e => e.Phone).IsRequired().HasColumnType("nvarchar(50)").HasColumnName("PHONE");
                entity.Property(e => e.StartAt).IsRequired().HasColumnType("datetime").HasColumnName("START_AT");
                entity.Property(e => e.FinishAt).IsRequired().HasColumnType("datetime").HasColumnName("FINISH_AT");

                entity.ToTable("WorkOrders");
            });
              

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasMany(e => e.Parts)
                .WithOne()
                .HasForeignKey(e => e.VisitId)
                .HasConstraintName("FK_VISIT_ID")
                .IsRequired();

                entity.Property(e => e.AssigneeFullName).IsRequired().HasColumnType("nvarchar(50)").HasColumnName("ASSIGNEE");
                entity.Property(e => e.AssignedFrom).IsRequired().HasColumnType("datetime").HasColumnName("ASSIGNED_AT");
                entity.Property(e => e.WorkOrderId).IsRequired().HasColumnName("FK_WORK_ORDER_ID");

                entity.ToTable("Visits");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.OwnsOne(e => e.Price).Property(p => p.Currency).HasConversion<string>().HasColumnType("nvarchar(10)").HasColumnName("CURRENCY");
                entity.OwnsOne(e => e.Price).Property(p => p.Amount).HasColumnType("decimal(18,2)").HasColumnName("AMOUNT");
                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
                entity.Property(e => e.Description).HasColumnType("nvarchar(500)").HasColumnName("DESCRIPTION");
                entity.Property(e => e.VisitId).HasColumnName("FK_VISIT_ID");

                entity.ToTable("Parts");

            });
        }

        public DbSet<WorkOrder> WorkOrders { get; init; }
        public DbSet<Part> Parts { get; init; }
        public DbSet<Visit> Visits { get; init; }

    }
}
