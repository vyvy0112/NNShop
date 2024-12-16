using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NNShop.Data;
using NNShop.Helpers;
using NNShop.ViewModels;


namespace NNShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly NnshopContext _context;

        public AdminController(NnshopContext context)
        {
            _context = context;
        }


        
        [HttpGet("/admin")]
        //public async Task<IActionResult> Index()
        //{
        //    // Lấy thông tin nhân viên đang đăng nhập
        //    var userName = User.Identity.Name; // Lấy tên từ Claims

        //    var nhanVien = _context.NhanViens.SingleOrDefault(nv => nv.MaNv.ToString() == userName);

        //    if (nhanVien == null || nhanVien.MaNv != 1)
        //    {
        //        return Redirect("/"); // Chuyển hướng về trang chủ nếu không phải admin
        //    }

        //    // Trả về giao diện admin nếu hợp lệ
        //    return View(await _context.NhanViens.ToListAsync());
        //}
        //// GET: Admin


        public async Task<IActionResult> Index()

        {
            var claimid = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID);
            var claimrole = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_ROLE);
            if (claimid == null)
            {
                return Unauthorized(); // Người dùng không hợp lệ
            }

            if (!int.TryParse(claimid.Value, out int staffid))
            {
                return BadRequest("Invalid staff ID."); // ID không hợp lệ
            }
            string role = claimrole?.Value;
            // Kiểm tra nhân viên hiện tại
            var nhanVien = await _context.NhanViens.FindAsync(staffid);
            if (staffid != nhanVien.MaNv)
            {
                return NotFound(); // Không khớp nhân viên
            }
            if (role != "Admin")

            {
                TempData["Message"] = $"Bạn không có quyền truy cập với tài khoản hiện tại ";
                return Redirect("/sorry");
            }
            return View(await _context.NhanViens.ToListAsync());
        }

        // GET: Admin/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: Admin/Create
        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("HoTen,MatKhau,NgaySinh")] NhanVien nhanVien)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(nhanVien);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(nhanVien);
        //}



        public async Task<IActionResult> Create(NhanVien model)
        {

            try
            {

                var nhanvien = new NhanVien
                {
                   HoTen = model.HoTen,
                   MatKhau = model.MatKhau,
                   NgaySinh = model.NgaySinh,
                   Role = "Staff",

                };

                _context.NhanViens.Add(nhanvien);
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




        // GET: Admin/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNv,HoTen,MatKhau,NgaySinh,Role")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.MaNv))
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
            return View(nhanVien);
        }

        // GET: Admin/Delete/5
        //[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.MaNv == id);
        }
    }
}
