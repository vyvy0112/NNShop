namespace NNShop.ViewModels
{
    public class CartItem
    {
        public string MaHh { get; set; }
        public string Hinh { get; set; }
        public string TenHH { get; set; }

        public double DonGia { get; set; }

        public double GiamGia { get; set; }
        public int SoLuong { get; set; }

        public double ThanhTien => SoLuong * (DonGia-GiamGia);

    }
}
