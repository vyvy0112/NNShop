using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NNShop.Data;

public partial class NnshopContext : DbContext
{
    public NnshopContext()
    {
    }

    public NnshopContext(DbContextOptions<NnshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiHh> LoaiHhs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.MaAm).HasName("PK__Admin__27247E4D9F37A6B5");

            entity.ToTable("Admin");

            entity.Property(e => e.MaAm)
                .ValueGeneratedNever()
                .HasColumnName("MaAM");
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTietDh).HasName("PK__ChiTietD__651E6E6A6D705492");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaChiTietDh).HasColumnName("MaChiTietDH");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaHh)
                .HasMaxLength(4)
                .HasColumnName("MaHH");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("FK__ChiTietDon__MaDH__7A3223E8");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaHH__7B264821");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__272586619567701A");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.MaTt).HasColumnName("MaTT");
            entity.Property(e => e.VanChuyen).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__DonHang__MaKH__756D6ECB");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaNV__76619304");

            entity.HasOne(d => d.MaTtNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaTt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaTT__7755B73D");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("khoachinh");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh)
                .HasMaxLength(4)
                .HasColumnName("MaHH");
            entity.Property(e => e.ChiTietHh).HasColumnName("ChiTietHH");
            entity.Property(e => e.Hinh).HasMaxLength(100);
            entity.Property(e => e.MaLoaiHh).HasColumnName("MaLoaiHH");
            entity.Property(e => e.TenHh).HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiHhNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoaiHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HangHoa__MaLoaiH__0C85DE4D");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__khachHan__2725CF1EA53702EC");

            entity.ToTable("khachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiHh>(entity =>
        {
            entity.HasKey(e => e.MaLoaiHh).HasName("PK__LoaiHH__122768DE96D81526");

            entity.ToTable("LoaiHH");

            entity.Property(e => e.MaLoaiHh)
                .ValueGeneratedNever()
                .HasColumnName("MaLoaiHH");
            entity.Property(e => e.TenLoaiHh)
                .HasMaxLength(50)
                .HasColumnName("TenLoaiHH");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A08C22E04");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTt).HasName("PK__TrangTha__272500792E6EC97D");

            entity.ToTable("TrangThai");

            entity.Property(e => e.MaTt)
                .ValueGeneratedNever()
                .HasColumnName("MaTT");
            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
