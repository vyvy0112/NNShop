using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class ChiTietDonHang
{
    public int MaChiTietDh { get; set; }

    public int? MaDh { get; set; }

    public string MaHh { get; set; } = null!;

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public double? TongTien { get; set; }

    public int? GiamGia { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual HangHoa MaHhNavigation { get; set; } = null!;
}
