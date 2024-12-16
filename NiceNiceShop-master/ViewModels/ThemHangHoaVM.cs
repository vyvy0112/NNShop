using NNShop.Data;
using System.ComponentModel.DataAnnotations;

namespace NNShop.ViewModels
{
    public class ThemHangHoaVM
    {

        [Key]
        [Display(Name = "Mã Hàng hóa")]
        [MaxLength(4, ErrorMessage = "Tối đa 4 kí tự")]
        [Required(ErrorMessage = "*")]
        public string MaHh { get; set; }

        [Display(Name = "Tên Hàng Hóa")]
        [Required(ErrorMessage = "*")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string TenHh { get; set; } = null!;

        [Display(Name = "Mã loại sản phẩm")]
        [Required(ErrorMessage = "*")]
        public int MaLoaiHh { get; set; }

        [Display(Name = "Mô tả")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")] // Điều chỉnh số ký tự cho phù hợp với trường mô tả
        public string? MoTa { get; set; }

        [Display(Name = "Chi tiết hàng hóa ")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")] // Điều chỉnh số ký tự cho phù hợp với trường mô tả
        public string? ChiTietHh { get; set; }

        [Display(Name = "Đơn Giá")]
        [Required(ErrorMessage = "*")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public double? DonGia { get; set; }

        [Display(Name = "Giảm Giá")]
        [Required(ErrorMessage = "*")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải trong khoảng từ 0 đến 100")]
        public double GiamGia { get; set; }

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuong { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Hinh { get; set; }

        // public virtual LoaiSp MaLoaiSpNavigation { get; set; } = null!;
        public List<LoaiHh> DSLoaiHh { get; set; } = new List<LoaiHh>();
    }
}



