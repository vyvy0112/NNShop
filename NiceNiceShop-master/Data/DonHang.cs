using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class DonHang
{
    public int MaDh { get; set; }

    public string? MaKh { get; set; }

    public DateOnly? NgayDat { get; set; }

    public DateOnly? NgayGiao { get; set; }

    public string? HoTen { get; set; }

    public string? Diachi { get; set; }

    public int MaNv { get; set; }

    public string? GhiChu { get; set; }

    public string? VanChuyen { get; set; }

    public double? TongTien { get; set; }

    public int MaTt { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    public virtual TrangThai MaTtNavigation { get; set; } = null!;
}
