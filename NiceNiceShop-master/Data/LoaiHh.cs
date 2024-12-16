using System;
using System.Collections.Generic;

namespace NNShop.Data;

public partial class LoaiHh
{
    public int MaLoaiHh { get; set; }

    public string TenLoaiHh { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
