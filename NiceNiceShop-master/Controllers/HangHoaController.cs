using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NNShop.Data;
using NNShop.ViewComponents;
using NNShop.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace NNShop.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly NnshopContext db;

        public HangHoaController(NnshopContext context)
        {
            db = context;
        }

       
        public IActionResult Index(int? loai, int page = 1)
        {
            int pageSize = 9; // Number of items per page
            var hangHoas = db.HangHoas.AsQueryable();

            // Filter by category if provided
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoaiHh == loai.Value);
            }

            // Get total number of products
            int totalItems = hangHoas.Count();

            // Apply pagination logic
            var paginatedHangHoas = hangHoas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new HangHoaVM
                {
                    MaHh = p.MaHh,
                    TenHH = p.TenHh,
                    DonGia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MoTa = p.MoTa ?? "",
                    TenLoai = p.MaLoaiHhNavigation.TenLoaiHh
                })
                .ToList();

            ViewBag.TotalProducts = hangHoas.Count();
            // Pass pagination details to the view
            var model = new PaginatedHangHoaVM
            {
                HangHoas = paginatedHangHoas,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return View(model);
        }


        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTa = p.MoTa ?? "",
                TenLoai = p.MaLoaiHhNavigation.TenLoaiHh


            });
            return View(result);
        }


        public IActionResult Detail(string id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiHhNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                MoTa = data.MoTa ?? String.Empty,
                Hinh = data.Hinh ?? String.Empty,
                TenLoai = data.MaLoaiHhNavigation.TenLoaiHh,
                SoLuong = data.SoLuong,
                GiamGia = data.GiamGia,
                ChiTiet = data.ChiTietHh ?? String.Empty

            };
            return View(result);
        }
    }
}
