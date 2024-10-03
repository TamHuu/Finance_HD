using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance_HD.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_LoaiThuChi",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_LoaiThuChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "cat_NoiDungThuChi",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MaLoaiThuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NoiBo = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_NoiDungThuChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fa_LoaiTien",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GiaTri = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fa_LoaiTien", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_BangKeNopTien",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaChiNhanhNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChiNhanhNop = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanNop = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoPhieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayNopTien = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiNopTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenNguoiNopTien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TyGia = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NguoiNhanTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaNoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaHinhThuc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_BangKeNopTien", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_BDSDNganHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Test = table.Column<bool>(type: "bit", nullable: true),
                    RequestID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_BDSDNganHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_ChiTietBangKeNhanVien",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaBangKe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_ChiTietBangKeNhanVien", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_ChiTietBangKeNopTien",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaBangKeNopTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaLoaiTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_ChiTietBangKeNopTien", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_DeNghiChi",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaChiNhanhDeNghi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChiNhanhChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanDeNghi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoPhieu = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayYeuCauNhanTien = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaNoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TyGia = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    HinhThucChi = table.Column<int>(type: "int", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    NguoiDuyet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayDuyet = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_DeNghiChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_HanMucCanhBao",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaBoPhan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HanMuc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_HanMucCanhBao", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_KhachHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HanMucNo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_KhachHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_LichSuGiaoDich",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaGiaoDich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayGiaoDich = table.Column<DateTime>(type: "datetime", nullable: true),
                    SoTaiKhoanDoiUng = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenTaiKhoanDoiUng = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TenNganHangDoiUng = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoaiTien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    SoTienSauGiaoDich = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaKenhThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoThamChieuKenh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SoTaiKhoanTruyVan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoaiGiaoDich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_LichSuGiaoDich", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_LoaiTaiKhoanNganHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_LoaiTaiKhoanNganHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_NganHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Ten = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    MaxLengthVA = table.Column<decimal>(type: "decimal(2,0)", nullable: true),
                    CodeVA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_NganHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_PhieuThuChi",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaLoaiThuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChiNhanhChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChiNhanhThu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanThu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaNoiDungThuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoPhieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayLapPhieu = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayDuyet = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiDuyet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiGiaoDich = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenNguoiGiaoDich = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChiNguoiGiaoDich = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TyGia = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    MaHinhThuc = table.Column<int>(type: "int", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoHoSoKemTheo = table.Column<int>(type: "int", nullable: true),
                    MaChungTuKeToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaBangKeNopTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GiamDoc = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    KeToanTruong = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiLapPhieu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiChiTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaDeNghi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhieuChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    NguoiNhanTien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenNguoiNhanTien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayChi = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DaNopQuy = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTaiKhoanGiaoDich = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoTaiKhoanKhachHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BillNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThuQuaDinhDanh = table.Column<bool>(type: "bit", nullable: true),
                    TenTaiKhoanKhachHang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_PhieuThuChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_TaiKhoanAo",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    MaTaiKhoanLienKet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VACode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_TaiKhoanAo", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_TaiKhoanNganHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaLoai = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoTaiKhoan = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DienGiai = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DongTienThu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DongTienChi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_TaiKhoanNganHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_ThuTienHang",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    SoHoaDon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayThu = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaKH = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HinhThucThu = table.Column<int>(type: "int", nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TyGia = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    SoTien = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    XacNhanThuTien = table.Column<bool>(type: "bit", nullable: true),
                    XacNhanNo = table.Column<bool>(type: "bit", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TienMat = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TienNganHang = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TienNo = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_ThuTienHang", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_TienTe",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_TienTe", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_TonSoQuy",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Nam = table.Column<int>(type: "int", nullable: true),
                    Thang = table.Column<int>(type: "int", nullable: true),
                    MaChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HinhThucGiaoDich = table.Column<int>(type: "int", nullable: true),
                    MaTaiKhoanGiaoDich = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoDuDauKy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThuTrongKy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChiTrongKy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChiTam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TonCuoiKy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoTaiKhoanKhachHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_TonSoQuy", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "fia_TrangThaiThuChi",
                columns: table => new
                {
                    Ma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fia_TrangThaiThuChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Branch",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhapNhan = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    CoSoQuy = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Branch", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Logo",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    logoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    logoImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    branchID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    index = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "sys_Menu",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ClassIcon = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ChildOfMenu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NhomGiaoDien = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongKham = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Target = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UsingFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Menu", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_OTP",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaToken = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OTP = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_OTP", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Permission",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FormAccess = table.Column<bool>(type: "bit", nullable: true),
                    PhanQuyenKho = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Permission", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_QuanLyTaiKhoanNH",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaTaiKhoanNganHang = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_QuanLyTaiKhoanNH", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Role",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Role", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_RoleBaoCao",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaBaoCao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_RoleBaoCao", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_RoleMenu",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllowAccess = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_RoleMenu", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_RolePermission",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RoleMenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_RolePermission", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_stt",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Colname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TableMax = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_STT", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Token",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SignToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DevicesID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastChangeIp = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastSeen = table.Column<DateTime>(type: "datetime", nullable: true),
                    ConnectionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsMobile = table.Column<bool>(type: "bit", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: true),
                    CreateMethod = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Token", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_User",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MaPhongBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaPhongBanKiemNhiem = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    MaChucDanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GioiTinh = table.Column<int>(type: "int", nullable: true),
                    HinhAnh = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    MSNV = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaDinhDanh = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CCCD = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EmailReceive = table.Column<bool>(type: "bit", nullable: true),
                    FCM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    MaKhuVuc = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    KhuVuc = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    QuayTiepNhan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MaPhongKhamChucNang = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PhongKhamDieuTri = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    GroupManager = table.Column<bool>(type: "bit", nullable: true),
                    UserManager = table.Column<bool>(type: "bit", nullable: true),
                    LdapLogin = table.Column<bool>(type: "bit", nullable: true),
                    CCHN = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LinkAnhCKDT = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    LinkAnhCKS = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_User", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_UserRole",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_UserRole", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "sys_Variable",
                columns: table => new
                {
                    Ma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_Variable", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ChucVu",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ChucVu", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_DanhSachBaoCao",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DanhSachBaoCao_1", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_HinhThucThuChi",
                columns: table => new
                {
                    Ma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_HinhThucThuChi", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_LoaiChungTu",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LoaiChungTu", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PhongBan",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaChiNhanh = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaBan = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoSoQuy = table.Column<bool>(type: "bit", nullable: true),
                    Ban = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PhongBan", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TyGia",
                columns: table => new
                {
                    Ma = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaTienTe = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TyGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayApDung = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TyGia", x => x.Ma);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_LoaiThuChi");

            migrationBuilder.DropTable(
                name: "cat_NoiDungThuChi");

            migrationBuilder.DropTable(
                name: "fa_LoaiTien");

            migrationBuilder.DropTable(
                name: "fia_BangKeNopTien");

            migrationBuilder.DropTable(
                name: "fia_BDSDNganHang");

            migrationBuilder.DropTable(
                name: "fia_ChiTietBangKeNhanVien");

            migrationBuilder.DropTable(
                name: "fia_ChiTietBangKeNopTien");

            migrationBuilder.DropTable(
                name: "fia_DeNghiChi");

            migrationBuilder.DropTable(
                name: "fia_HanMucCanhBao");

            migrationBuilder.DropTable(
                name: "fia_KhachHang");

            migrationBuilder.DropTable(
                name: "fia_LichSuGiaoDich");

            migrationBuilder.DropTable(
                name: "fia_LoaiTaiKhoanNganHang");

            migrationBuilder.DropTable(
                name: "fia_NganHang");

            migrationBuilder.DropTable(
                name: "fia_PhieuThuChi");

            migrationBuilder.DropTable(
                name: "fia_TaiKhoanAo");

            migrationBuilder.DropTable(
                name: "fia_TaiKhoanNganHang");

            migrationBuilder.DropTable(
                name: "fia_ThuTienHang");

            migrationBuilder.DropTable(
                name: "fia_TienTe");

            migrationBuilder.DropTable(
                name: "fia_TonSoQuy");

            migrationBuilder.DropTable(
                name: "fia_TrangThaiThuChi");

            migrationBuilder.DropTable(
                name: "sys_Branch");

            migrationBuilder.DropTable(
                name: "sys_Logo");

            migrationBuilder.DropTable(
                name: "sys_Menu");

            migrationBuilder.DropTable(
                name: "sys_OTP");

            migrationBuilder.DropTable(
                name: "sys_Permission");

            migrationBuilder.DropTable(
                name: "sys_QuanLyTaiKhoanNH");

            migrationBuilder.DropTable(
                name: "sys_Role");

            migrationBuilder.DropTable(
                name: "sys_RoleBaoCao");

            migrationBuilder.DropTable(
                name: "sys_RoleMenu");

            migrationBuilder.DropTable(
                name: "sys_RolePermission");

            migrationBuilder.DropTable(
                name: "sys_stt");

            migrationBuilder.DropTable(
                name: "sys_Token");

            migrationBuilder.DropTable(
                name: "sys_User");

            migrationBuilder.DropTable(
                name: "sys_UserRole");

            migrationBuilder.DropTable(
                name: "sys_Variable");

            migrationBuilder.DropTable(
                name: "tbl_ChucVu");

            migrationBuilder.DropTable(
                name: "tbl_DanhSachBaoCao");

            migrationBuilder.DropTable(
                name: "tbl_HinhThucThuChi");

            migrationBuilder.DropTable(
                name: "tbl_LoaiChungTu");

            migrationBuilder.DropTable(
                name: "tbl_PhongBan");

            migrationBuilder.DropTable(
                name: "tbl_TyGia");
        }
    }
}
