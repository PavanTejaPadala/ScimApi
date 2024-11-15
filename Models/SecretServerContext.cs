using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ScimApi.Models;

public partial class SecretServerContext : DbContext
{
    public SecretServerContext()
    {
    }

    public SecretServerContext(DbContextOptions<SecretServerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbGroup> TbGroups { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GVC1387\\MSSQLSERVER2019;Initial Catalog=SecretServer;Persist Security Info=True;User ID=sa;Password=cybage@123456;Encrypt=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__tbGroup__149AF30A47AF8C8B");

            entity.ToTable("tbGroup");

            entity.Property(e => e.GroupId)
                .ValueGeneratedNever()
                .HasColumnName("GroupID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.GroupName).HasMaxLength(100);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tbUser__1788CC4CC1A35F5B");

            entity.ToTable("tbUser");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
