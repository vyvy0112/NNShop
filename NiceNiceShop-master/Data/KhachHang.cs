using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public int? Diem { get; set; }

    public bool GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
