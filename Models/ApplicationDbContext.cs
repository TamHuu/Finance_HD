using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Finance_HD.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }


        public virtual DbSet<CatLoaiThuChi> CatLoaiThuChi { get; set; }

        public virtual DbSet<CatNoiDungThuChi> CatNoiDungThuChi { get; set; }

        public virtual DbSet<FaLoaiTien> FaLoaiTien { get; set; }

        public virtual DbSet<FiaBangKeNopTien> FiaBangKeNopTien { get; set; }

        public virtual DbSet<FiaBdsdnganHang> FiaBdsdnganHang { get; set; }

        public virtual DbSet<FiaChiTietBangKeNhanVien> FiaChiTietBangKeNhanVien { get; set; }

        public virtual DbSet<FiaChiTietBangKeNopTien> FiaChiTietBangKeNopTien { get; set; }

        public virtual DbSet<FiaDeNghiChi> FiaDeNghiChi { get; set; }

        public virtual DbSet<FiaHanMucCanhBao> FiaHanMucCanhBao { get; set; }

        public virtual DbSet<FiaKhachHang> FiaKhachHang { get; set; }

        public virtual DbSet<FiaLichSuGiaoDich> FiaLichSuGiaoDich { get; set; }

        public virtual DbSet<FiaLoaiTaiKhoanNganHang> FiaLoaiTaiKhoanNganHang { get; set; }

        public virtual DbSet<FiaNganHang> FiaNganHang { get; set; }

        public virtual DbSet<FiaPhieuThuChi> FiaPhieuThuChi { get; set; }

        public virtual DbSet<FiaTaiKhoanAo> FiaTaiKhoanAo { get; set; }

        public virtual DbSet<FiaTaiKhoanNganHang> FiaTaiKhoanNganHang { get; set; }

        public virtual DbSet<FiaThuTienHang> FiaThuTienHang { get; set; }

        public virtual DbSet<FiaTienTe> FiaTienTe { get; set; }

        public virtual DbSet<FiaTonSoQuy> FiaTonSoQuy { get; set; }

        public virtual DbSet<FiaTrangThaiThuChi> FiaTrangThaiThuChi { get; set; }

        public virtual DbSet<SysBranch> SysBranch { get; set; }

        public virtual DbSet<SysLogo> SysLogo { get; set; }

        public virtual DbSet<SysMenu> SysMenu { get; set; }

        public virtual DbSet<SysOtp> SysOtp { get; set; }

        public virtual DbSet<SysPermission> SysPermission { get; set; }

        public virtual DbSet<SysQuanLyTaiKhoanNh> SysQuanLyTaiKhoanNh { get; set; }

        public virtual DbSet<SysRole> SysRole { get; set; }

        public virtual DbSet<SysRoleBaoCao> SysRoleBaoCao { get; set; }

        public virtual DbSet<SysRoleMenu> SysRoleMenu { get; set; }

        public virtual DbSet<SysRolePermission> SysRolePermission { get; set; }

        public virtual DbSet<SysStt> SysStt { get; set; }

        public virtual DbSet<SysToken> SysToken { get; set; }

        public virtual DbSet<SysUser> SysUser { get; set; }

        public virtual DbSet<SysUserRole> SysUserRole { get; set; }

        public virtual DbSet<SysVariable> SysVariable { get; set; }

        public virtual DbSet<TblChucVu> TblChucVu { get; set; }

        public virtual DbSet<TblDanhSachBaoCao> TblDanhSachBaoCao { get; set; }

        public virtual DbSet<TblHinhThucThuChi> TblHinhThucThuChi { get; set; }

        public virtual DbSet<TblLoaiChungTu> TblLoaiChungTu { get; set; }

        public virtual DbSet<TblPhongBan> TblPhongBan { get; set; }

        public virtual DbSet<TblTyGia> TblTyGia { get; set; }

        public virtual DbSet<VPermission> VPermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("HDFA");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatLoaiThuChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("cat_LoaiThuChi");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(1000);
            });

            modelBuilder.Entity<CatNoiDungThuChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("cat_NoiDungThuChi");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(1000);
            });

            modelBuilder.Entity<FaLoaiTien>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fa_LoaiTien");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<FiaBangKeNopTien>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_BangKeNopTien");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.LyDo).HasMaxLength(500);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayLap).HasColumnType("datetime");
                entity.Property(e => e.NgayNopTien).HasColumnType("datetime");
                entity.Property(e => e.SoPhieu).HasMaxLength(50);
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TenNguoiNopTien).HasMaxLength(500);
                entity.Property(e => e.TyGia).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<FiaBdsdnganHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_BDSDNganHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Error).HasMaxLength(300);
                entity.Property(e => e.NoiDung).HasMaxLength(2000);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(100)
                    .HasColumnName("RequestID");
            });

            modelBuilder.Entity<FiaChiTietBangKeNhanVien>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_ChiTietBangKeNhanVien");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<FiaChiTietBangKeNopTien>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_ChiTietBangKeNopTien");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<FiaDeNghiChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_DeNghiChi");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayDuyet).HasColumnType("datetime");
                entity.Property(e => e.NgayLap).HasColumnType("datetime");
                entity.Property(e => e.NgayYeuCauNhanTien).HasColumnType("datetime");
                entity.Property(e => e.SoPhieu)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TyGia).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<FiaHanMucCanhBao>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_HanMucCanhBao");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.HanMuc).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<FiaKhachHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_KhachHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.HanMucNo).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<FiaLichSuGiaoDich>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_LichSuGiaoDich");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.LoaiGiaoDich).HasMaxLength(50);
                entity.Property(e => e.LoaiTien).HasMaxLength(50);
                entity.Property(e => e.MaGiaoDich).HasMaxLength(50);
                entity.Property(e => e.MaKenhThanhToan).HasMaxLength(50);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayGiaoDich).HasColumnType("datetime");
                entity.Property(e => e.SoTaiKhoanDoiUng).HasMaxLength(50);
                entity.Property(e => e.SoTaiKhoanTruyVan).HasMaxLength(50);
                entity.Property(e => e.SoThamChieuKenh).HasMaxLength(50);
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.SoTienSauGiaoDich).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TenNganHangDoiUng).HasMaxLength(500);
                entity.Property(e => e.TenTaiKhoanDoiUng).HasMaxLength(500);
            });

            modelBuilder.Entity<FiaLoaiTaiKhoanNganHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_LoaiTaiKhoanNganHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(300);
            });

            modelBuilder.Entity<FiaNganHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_NganHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CodeVa)
                    .HasMaxLength(50)
                    .HasColumnName("CodeVA");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.MaxLengthVa)
                    .HasColumnType("decimal(2, 0)")
                    .HasColumnName("MaxLengthVA");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(300);
            });

            modelBuilder.Entity<FiaPhieuThuChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_PhieuThuChi");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.BillNumber).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DiaChiNguoiGiaoDich).HasMaxLength(500);
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.LyDo).HasMaxLength(500);
                entity.Property(e => e.MaChungTuKeToan).HasMaxLength(50);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayChi).HasColumnType("datetime");
                entity.Property(e => e.NgayDuyet).HasColumnType("datetime");
                entity.Property(e => e.NgayLapPhieu).HasColumnType("datetime");
                entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(100)
                    .HasColumnName("RequestID");
                entity.Property(e => e.SoPhieu).HasMaxLength(50);
                entity.Property(e => e.SoTaiKhoanKhachHang).HasMaxLength(50);
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TenNguoiGiaoDich).HasMaxLength(500);
                entity.Property(e => e.TenNguoiNhanTien).HasMaxLength(200);
                entity.Property(e => e.TenTaiKhoanKhachHang).HasMaxLength(200);
                entity.Property(e => e.TyGia).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<FiaTaiKhoanAo>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_TaiKhoanAo");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(300);
                entity.Property(e => e.Vacode)
                    .HasMaxLength(50)
                    .HasColumnName("VACode");
            });

            modelBuilder.Entity<FiaTaiKhoanNganHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_TaiKhoanNganHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DienGiai).HasMaxLength(300);
                entity.Property(e => e.Mid)
                    .HasMaxLength(50)
                    .HasColumnName("MID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(100);
                entity.Property(e => e.Pid)
                    .HasMaxLength(50)
                    .HasColumnName("PID");
                entity.Property(e => e.SoTaiKhoan).HasMaxLength(300);
                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<FiaThuTienHang>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_ThuTienHang");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.GhiChu).HasMaxLength(500);
                entity.Property(e => e.MaKh).HasColumnName("MaKH");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayThu).HasColumnType("datetime");
                entity.Property(e => e.SoHoaDon).HasMaxLength(50);
                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TienMat).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TienNganHang).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TienNo).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TyGia).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<FiaTienTe>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_TienTe");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(50);
            });

            modelBuilder.Entity<FiaTonSoQuy>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_TonSoQuy");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ChiTam).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ChiTrongKy).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SoDuDauKy).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.SoTaiKhoanKhachHang).HasMaxLength(50);
                entity.Property(e => e.ThuTrongKy).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TonCuoiKy).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<FiaTrangThaiThuChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("fia_TrangThaiThuChi");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(50);
            });

            modelBuilder.Entity<SysBranch>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Branch");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.Logo).HasMaxLength(500);
                entity.Property(e => e.MaSoThue).HasMaxLength(50);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.PhapNhan).HasMaxLength(600);
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<SysLogo>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("sys_Logo");

                entity.Property(e => e.BranchId).HasColumnName("branchID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.Index).HasColumnName("index");
                entity.Property(e => e.LogoImage)
                    .HasMaxLength(50)
                    .HasColumnName("logoImage");
                entity.Property(e => e.LogoName)
                    .HasMaxLength(50)
                    .HasColumnName("logoName");
                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SysMenu>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Menu");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ClassIcon)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Link).HasMaxLength(200);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.ParentId).HasColumnName("ParentID");
                entity.Property(e => e.UsingFor).HasMaxLength(50);
            });

            modelBuilder.Entity<SysOtp>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_OTP");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Otp)
                    .HasMaxLength(500)
                    .HasColumnName("OTP");
            });

            modelBuilder.Entity<SysPermission>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Permission");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(300);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<SysQuanLyTaiKhoanNh>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_QuanLyTaiKhoanNH");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Role");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(300);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<SysRoleBaoCao>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_RoleBaoCao");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SysRoleMenu>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_RoleMenu");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.MenuId).HasColumnName("MenuID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            modelBuilder.Entity<SysRolePermission>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_RolePermission");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
                entity.Property(e => e.RoleMenuId).HasColumnName("RoleMenuID");
            });

            modelBuilder.Entity<SysStt>(entity =>
            {
                entity.HasKey(e => e.Ma).HasName("PK_sys_STT");

                entity.ToTable("sys_stt");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Colname).HasMaxLength(200);
                entity.Property(e => e.CreateDate).HasColumnType("date");
                entity.Property(e => e.TableMax).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.TableName).HasMaxLength(50);
            });

            modelBuilder.Entity<SysToken>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Token");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ConnectionId)
                    .HasMaxLength(50)
                    .HasColumnName("ConnectionID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeviceName).HasMaxLength(150);
                entity.Property(e => e.DevicesId)
                    .HasMaxLength(50)
                    .HasColumnName("DevicesID");
                entity.Property(e => e.Ip)
                    .HasMaxLength(20)
                    .HasColumnName("IP");
                entity.Property(e => e.LastChangeIp).HasColumnType("datetime");
                entity.Property(e => e.LastSeen).HasColumnType("datetime");
                entity.Property(e => e.RefreshToken).HasMaxLength(500);
                entity.Property(e => e.SignToken).HasMaxLength(500);
                entity.Property(e => e.Token).HasMaxLength(500);
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_User");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.Cccd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");
                entity.Property(e => e.Cchn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CCHN");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.DiaChi).HasMaxLength(500);
                entity.Property(e => e.Fcm)
                    .HasMaxLength(200)
                    .HasColumnName("FCM");
                entity.Property(e => e.FullName).HasMaxLength(300);
                entity.Property(e => e.HinhAnh)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.KhuVuc)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.LinkAnhCkdt)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("LinkAnhCKDT");
                entity.Property(e => e.LinkAnhCks)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("LinkAnhCKS");
                entity.Property(e => e.MaDinhDanh)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MaPhongKhamChucNang)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Msnv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MSNV");
                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
                entity.Property(e => e.NgaySinh).HasColumnType("datetime");
                entity.Property(e => e.NgayVaoLam).HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(300);
                entity.Property(e => e.PhongKhamDieuTri)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.QuayTiepNhan)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<SysUserRole>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_UserRole");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<SysVariable>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("sys_Variable");

                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<TblChucVu>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("tbl_ChucVu");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<TblDanhSachBaoCao>(entity =>
            {
                entity.HasKey(e => e.Ma).HasName("PK_tbl_DanhSachBaoCao_1");

                entity.ToTable("tbl_DanhSachBaoCao");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.MenuId).HasColumnName("MenuID");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<TblHinhThucThuChi>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("tbl_HinhThucThuChi");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(50);
            });

            modelBuilder.Entity<TblLoaiChungTu>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("tbl_LoaiChungTu");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<TblPhongBan>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("tbl_PhongBan");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Ten).HasMaxLength(500);
            });

            modelBuilder.Entity<TblTyGia>(entity =>
            {
                entity.HasKey(e => e.Ma);

                entity.ToTable("tbl_TyGia");

                entity.Property(e => e.Ma).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DeletedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NgayApDung).HasColumnType("datetime");
                entity.Property(e => e.TyGia).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VPermission>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vPermission");

                entity.Property(e => e.BranchId).HasColumnName("BranchID");
                entity.Property(e => e.FullName).HasMaxLength(300);
                entity.Property(e => e.MenuId).HasColumnName("MenuID");
                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}