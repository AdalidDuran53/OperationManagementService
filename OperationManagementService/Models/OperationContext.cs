using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OperationManagementService.Models;

public partial class OperationContext : DbContext
{
    public OperationContext()
    {
    }

    public OperationContext(DbContextOptions<OperationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    public virtual DbSet<SessionLog> SessionLogs { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC648B24BC39");

            entity.ToTable("OperationLog");

            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.Request).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Response).HasColumnType("nvarchar(max)");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");

            entity.HasOne(d => d.Session).WithMany(p => p.OperationLogs)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Operation__Respo__3E52440B");
        });

        modelBuilder.Entity<SessionLog>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__SessionL__C9F49270AAFBA4AE");

            entity.ToTable("SessionLog");

            entity.Property(e => e.SessionId)
                .ValueGeneratedNever()
                .HasColumnName("SessionID");
            entity.Property(e => e.EndSession).HasColumnType("datetime");
            entity.Property(e => e.InitSession).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SessionLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SessionLo__EndSe__3B75D760");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B8D2BE394");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Operation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OperationId)
                .HasConstraintName("FK__Transacti__isDel__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC9ED23106");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F284561E94650C").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.PasswordHash).HasColumnType("nvarchar(max)");
            entity.Property(e => e.PasswordSalst).HasColumnType("nvarchar(max)");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
