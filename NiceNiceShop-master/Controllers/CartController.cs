using Microsoft.AspNetCore.Mvc;
using NNShop.Data;
using NNShop.ViewModels;
using NNShop.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace NNShop.Controllers
{
    public class CartController : Controller
    {
        private readonly NnshopContext db;

        public CartController(NnshopContext context)
        {
            db = context;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        [Authorize]
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity,
                    GiamGia = hangHoa.GiamGia
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(string id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Tang(string id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                item.SoLuong += 1;
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Giam(string id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                item.SoLuong -= 1;
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/HangHoa");
            }

            return View(Cart);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var khachhang = new KhachHang();
                var nhanvien = new NhanVien();
                var staffId = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID).Value);
                if (model.giongnhannvien)
                {
                    nhanvien = db.NhanViens.SingleOrDefault(nv => nv.MaNv == staffId);
                }
                var hoadon = new DonHang
                {
                    MaNv = staffId,
                    MaKh = model.MaKH,
                    HoTen = model.HoTen ?? khachhang.HoTen,
                    Diachi = model.DiaChi ?? khachhang.DiaChi,
                    NgayDat = DateOnly.FromDateTime(DateTime.Now),
                    NgayGiao = DateOnly.FromDateTime(DateTime.Now),
                    VanChuyen = "Nhận Tại quầy",
                    MaTt = 1,
                    GhiChu = model.GhiChu,
                    TongTien = Cart.Sum(item => (item.DonGia - item.GiamGia) * item.SoLuong)
                };

                db.Database.BeginTransaction();
                try
                {
                    db.AddRange(hoadon);
                    await db.SaveChangesAsync();

                    var cthds = new List<ChiTietDonHang>();
                    foreach (var item in Cart)
                    {
                        // Lấy sản phẩm từ cơ sở dữ liệu
                        var hangHoa = db.HangHoas.SingleOrDefault(hh => hh.MaHh == item.MaHh);
                        if (hangHoa == null || hangHoa.SoLuong < item.SoLuong)
                        {
                            throw new Exception($"Không đủ số lượng hàng hóa: {item.TenHH}");
                        }

                        // Giảm số lượng sản phẩm
                        hangHoa.SoLuong -= item.SoLuong;
                        // Tính đơn giá sau giảm giá
                        var donGiaSauGiam = item.DonGia - item.GiamGia;

                        // Đảm bảo đơn giá không âm
                        if (donGiaSauGiam < 0)
                        {
                            throw new Exception($"Giảm giá không hợp lệ cho sản phẩm: {item.TenHH}");
                        }

                        // Tạo chi tiết đơn hàng
                        cthds.Add(new ChiTietDonHang
                        {
                            MaDh = hoadon.MaDh,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
                            MaHh = item.MaHh,
                            GiamGia = (int?)Math.Round(item.GiamGia) * item.SoLuong,
                            TongTien = donGiaSauGiam * item.SoLuong

                        });
                    }
                    db.UpdateRange(db.HangHoas);
                    db.AddRange(cthds);
                    await db.SaveChangesAsync();

                    db.Database.CommitTransaction();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");

                }
                catch
                {
                    if (db.Database.CurrentTransaction != null)
                    {
                        db.Database.RollbackTransaction();
                    }

                    return View("");
                }
            }

            return View(Cart);
        }
    }
}


