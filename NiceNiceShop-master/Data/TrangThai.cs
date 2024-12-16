using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class TrangThai
{
    public int MaTt { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
