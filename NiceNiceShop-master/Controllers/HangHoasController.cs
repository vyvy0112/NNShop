using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NNShop.Data;
using NNShop.ViewModels;
using NNShop.Helpers;

namespace NNShop.Controllers
{
    public class HangHoasController : Controller
    {
        private readonly NnshopContext _context;

        public HangHoasController(NnshopContext context)
        {
            _context = context;
        }

        // GET: HangHoas
        public async Task<IActionResult> Index()
        {
            var nnshopContext = _context.HangHoas.Include(h => h.MaLoaiHhNavigation);
            return View(await nnshopContext.ToListAsync());
        }

        // GET: HangHoas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiHhNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        public IActionResult Create()
        {
            var loaiSanPhams = _context.LoaiHhs.ToList();
            if (loaiSanPhams == null || !loaiSanPhams.Any())
            {
                // Gán danh sách rỗng và có thể log thông báo
                loaiSanPhams = new List<LoaiHh>();
                Console.WriteLine("Danh sách loại sản phẩm bị null hoặc không có dữ liệu.");
            }

            var model = new ThemHangHoaVM
            {
                DSLoaiHh = loaiSanPhams
            };


            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> Create(ThemHangHoaVM model, IFormFile Hinh)
        {
            var loaiSanPhams = _context.LoaiHhs?.ToList();
            Console.WriteLine(loaiSanPhams?.Count ?? 0); // In ra số lượng phần tử

            try
            {
                // Mapping từ model sang entity

                var hangHoa = new HangHoa
                {
                    MaHh = model.MaHh,
                    TenHh = model.TenHh,
                    MaLoaiHh = model.MaLoaiHh,
                    MoTa = model.MoTa,
                    DonGia = model.DonGia,
                    GiamGia = model.GiamGia,
                    SoLuong = model.SoLuong,
                };


                // Xử lý hình ảnh nếu có
                if (Hinh != null)
                {
                    var hinhPath = MyUtil.UploadHinh(Hinh, "HangHoa");
                    if (!string.IsNullOrEmpty(hinhPath))
                    {
                        hangHoa.Hinh = hinhPath;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể upload hình ảnh");
                        return View(model);
                    }
                }

                // Thêm vào DbContext và lưu thay đổi
                _context.HangHoas.Add(hangHoa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                // Ghi log hoặc hiển thị lỗi
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }


            // Nếu ModelState không hợp lệ hoặc lỗi xảy ra
            return View(model);
        }


        // GET: HangHoas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiHh"] = new SelectList(_context.LoaiHhs, "MaLoaiHh", "MaLoaiHh", hangHoa.MaLoaiHh);
            return View(hangHoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ThemHangHoaVM model, string id, IFormFile Hinh)
        {
            try
            {
                var sanpham = await _context.HangHoas.SingleOrDefaultAsync(sp => sp.MaHh == id);
                if (sanpham == null)
                {
                    return NotFound();
                }

                // Kiểm tra nếu `id` không khớp
                if (id != sanpham.MaHh)
                {
                    return BadRequest();
                }
                sanpham.TenHh = model.TenHh;
                sanpham.MaLoaiHh = model.MaLoaiHh;
                sanpham.MoTa = model.MoTa;
                sanpham.ChiTietHh = model.ChiTietHh;
                sanpham.DonGia = model.DonGia;
                sanpham.GiamGia = model.GiamGia;
                sanpham.SoLuong = model.SoLuong;

                _context.HangHoas.Update(sanpham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                ViewData["MaLoaiHh"] = new SelectList(_context.LoaiHhs, "MaLoaiHh", "MaLoaiHh", model.MaLoaiHh);
                return View(model);
            }
        }


        // GET: HangHoas/Delete/5
        public async Task<IActionResult> Delete(string id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hangHoa = await _context.HangHoas
                    .Include(h => h.MaLoaiHhNavigation)
                    .FirstOrDefaultAsync(m => m.MaHh == id);
                if (hangHoa == null)
                {
                    return NotFound();
                }

                return View(hangHoa);
            }

            // POST: HangHoas/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(string id)
            {
                var hangHoa = await _context.HangHoas.FindAsync(id);
                if (hangHoa != null)
                {
                    _context.HangHoas.Remove(hangHoa);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool HangHoaExists(string id)
            {
                return _context.HangHoas.Any(e => e.MaHh == id);
            }
        }
    }
