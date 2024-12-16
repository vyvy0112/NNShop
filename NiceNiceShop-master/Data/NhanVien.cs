using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string HoTen { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public DateOnly NgaySinh { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
