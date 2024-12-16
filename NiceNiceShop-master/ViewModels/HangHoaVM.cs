namespace NNShop.ViewModels
{
    public class HangHoaVM
    {
        public string MaHh { get; set; }

        public string TenHH { get; set; }

        public string Hinh { get; set; }

        public double DonGia { get; set; }

        public string MoTa { get; set; }

        public string TenLoai { get; set; }


    }
    public class ChiTietHangHoaVM
    {
        public string MaHh { get; set; }
        public string TenHH { get; set; }

        public string Hinh { get; set; }

        public double DonGia { get; set; }

        public string MoTa { get; set; }

        public string TenLoai { get; set; }

        public int SoLuong { get; set; }

        public string ChiTiet { get; set; }

        public double GiamGia { get; set; }


    }
    public class PaginatedHangHoaVM
    {
        public IEnumerable<HangHoaVM> HangHoas { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
