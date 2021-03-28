using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Survey.Data.Entities
{
    public partial class SantaPoneCentralDatabaseContext : DbContext
    {
        public SantaPoneCentralDatabaseContext()
        {
        }

        public SantaPoneCentralDatabaseContext(DbContextOptions<SantaPoneCentralDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientStatus> ClientStatuses { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<YuleLog> YuleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "app");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("categoryDescription");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "app");

                entity.HasIndex(e => e.ClientStatusId, "ClientStatusIndex");

                entity.HasIndex(e => e.Email, "UQ__Client__AB6E6164ED3C4937")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientID");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("addressLine1");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(50)
                    .HasColumnName("addressLine2");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("clientName");

                entity.Property(e => e.ClientStatusId).HasColumnName("clientStatusID");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.HasAccount).HasColumnName("hasAccount");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .HasColumnName("nickname");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("postalCode");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.HasOne(d => d.ClientStatus)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.ClientStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__clientSt__31907326");
            });

            modelBuilder.Entity<ClientStatus>(entity =>
            {
                entity.ToTable("ClientStatus", "app");

                entity.Property(e => e.ClientStatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientStatusID");

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("statusDescription");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note", "app");

                entity.HasIndex(e => e.ClientId, "NoteClientIndex");

                entity.Property(e => e.NoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("noteID");

                entity.Property(e => e.ClientId).HasColumnName("clientID");

                entity.Property(e => e.NoteContents)
                    .HasMaxLength(2000)
                    .HasColumnName("noteContents");

                entity.Property(e => e.NoteSubject)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("noteSubject");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Note__clientID__0453B85B");
            });

            modelBuilder.Entity<YuleLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__YuleLog__7839F62DD7E28393");

                entity.ToTable("YuleLog", "app");

                entity.Property(e => e.LogId)
                    .ValueGeneratedNever()
                    .HasColumnName("logID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasColumnName("logDate");

                entity.Property(e => e.LogText)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("logText");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.YuleLogs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__YuleLog__categor__1C2B41EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
