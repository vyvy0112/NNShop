using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NNShop.Data;
using NNShop.ViewModels;
namespace NNShop.Controllers
{
    [Authorize]
    public class DonHangController : Controller
    {
        private readonly NnshopContext _context;

        public DonHangController(NnshopContext context)
        {
            _context = context;
        }

        // GET: DonHang
        public async Task<IActionResult> Index()
        {
            var nnshopContext = _context.DonHangs.Include(d => d.MaKhNavigation).Include(d => d.MaNvNavigation).Include(d => d.MaTtNavigation);
            return View(await nnshopContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin đơn hàng và chi tiết đơn hàng liên quan
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs) // Bao gồm chi tiết đơn hàng
                .ThenInclude(ct => ct.MaHhNavigation) // Bao gồm thông tin hàng hóa
                .Include(d => d.MaKhNavigation) // Bao gồm thông tin khách hàng
                .Include(d => d.MaNvNavigation) // Bao gồm thông tin nhân viên
                .Include(d => d.MaTtNavigation) // Bao gồm thông tin trạng thái
                .FirstOrDefaultAsync(d => d.MaDh == id);

            if (donHang == null)
            {
                return NotFound();
            }

            // Tính tổng tiền chi tiết đơn hàng
            double? tongThanhTien = donHang.ChiTietDonHangs.Sum(ct => ct.TongTien);

            // Gửi tổng tiền qua ViewData
            ViewData["TongThanhTien"] = tongThanhTien;

            return View(donHang);
        }



        // GET: DonHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", donHang.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", donHang.MaNv);
            ViewData["MaTt"] = new SelectList(_context.TrangThais, "MaTt", "MaTt", donHang.MaTt);
            return View(donHang);
        }

        // POST: DonHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDh,MaKh,NgayDat,NgayGiao,HoTen,Diachi,MaNv,GhiChu,VanChuyen,TongTien,MaTt")] DonHang donHang)
        {
            if (id != donHang.MaDh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", donHang.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", donHang.MaNv);
            ViewData["MaTt"] = new SelectList(_context.TrangThais, "MaTt", "MaTt", donHang.MaTt);
            return View(donHang);
        }

        // GET: DonHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaNvNavigation)
                .Include(d => d.MaTtNavigation)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDh == id);
        }


        public async Task<IActionResult> ThongKeSanPham()
        {
            var thongKeSanPham = await _context.ChiTietDonHangs
            .GroupBy(ct => ct.MaHh)
            .Select(g => new
            {
                MaHh = g.Key,
                TenHh = g.FirstOrDefault().MaHhNavigation.TenHh,
                SoLuongBan = g.Sum(ct => ct.SoLuong),
                TongDoanhThu = g.Sum(ct => ct.TongTien)
            })
                .OrderByDescending(r => r.SoLuongBan)
                .ToListAsync();

            return View(thongKeSanPham);
        }

        // Thống kê doanh thu theo ngày
        public async Task<IActionResult> ThongKeDoanhThuTheoNgay()
        {
            var doanhThuTheoNgay = await _context.DonHangs
                .GroupBy(d => d.NgayDat)
                .Select(g => new ThongKeViewModel
                {
                    // Kiểm tra null trước khi chuyển đổi
                    Ngay = g.Key.HasValue ? g.Key.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    TongDoanhThu = g.Sum(d => d.TongTien) ?? 0
                })
                .OrderBy(r => r.Ngay)
                .ToListAsync();

            return View("ThongKeIndex", doanhThuTheoNgay);
        }



        // Thống kê doanh thu theo tháng
        public async Task<IActionResult> ThongKeDoanhThuTheoThang()
        {
            var doanhThuTheoThang = await _context.DonHangs
                .Where(d => d.NgayDat.HasValue)
                .GroupBy(d => new { d.NgayDat.Value.Year, d.NgayDat.Value.Month })
                .Select(g => new ThongKeViewModel
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongDoanhThu = g.Sum(d => d.TongTien) ?? 0
                })
                .OrderBy(r => r.Nam).ThenBy(r => r.Thang)
                .ToListAsync();

            return View("ThongKeIndex", doanhThuTheoThang);
        }

        // Thống kê doanh thu theo năm
        public async Task<IActionResult> ThongKeDoanhThuTheoNam()
        {
            var doanhThuTheoNam = await _context.DonHangs
                .Where(d => d.NgayDat.HasValue)
                .GroupBy(d => d.NgayDat.Value.Year)
                .Select(g => new ThongKeViewModel
                {
                    Nam = g.Key,
                    TongDoanhThu = g.Sum(d => d.TongTien) ?? 0
                })
                .OrderBy(r => r.Nam)
                .ToListAsync();

            return View("ThongKeIndex", doanhThuTheoNam);
        }

        // Trang Thống kê
        public IActionResult ThongKeIndex()
        {
            return View();
        }
    }






}

