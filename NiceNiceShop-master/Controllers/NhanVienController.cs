using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NNShop.Data;
using NNShop.Helpers;
using NNShop.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace NNShop.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly NnshopContext db;
        public NhanVienController(NnshopContext context)
        {
            db = context;
        }

        #region login
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var nhanVien = db.NhanViens.SingleOrDefault(nv => nv.MaNv == model.UserName);
                if (nhanVien == null)
                {
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                }
                else
                {
                    if (nhanVien.MatKhau != model.Password)
                    {
                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                    }
                    else
                    {
                        var claims = new List<Claim> {
                                new Claim(ClaimTypes.Name, nhanVien.HoTen),
                                new Claim(MySetting.CLAIM_ROLE, nhanVien.Role),
                                new Claim(ClaimTypes.Role, nhanVien.Role),
                                new Claim(MySetting.CLAIM_STAFFID, nhanVien.MaNv.ToString()),



                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Lấy staff ID từ Claims
            var claim = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID);
            if (claim == null)
            {
                return Unauthorized(); // Người dùng không hợp lệ
            }

            if (!int.TryParse(claim.Value, out int staffid))
            {
                return BadRequest("Invalid staff ID."); // ID không hợp lệ
            }

            // Lấy nhân viên theo ID
            var nhanVien = await db.NhanViens.FindAsync(staffid);
            if (nhanVien == null)
            {
                return NotFound(); // Không tìm thấy nhân viên
            }

            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile([Bind("MaNv,HoTen,MatKhau,NgaySinh,Role")] NhanVien nhanVien)
        {
            // Lấy staff ID từ Claims
            var claim = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID);
            if (claim == null)
            {
                return Unauthorized(); // Người dùng không hợp lệ
            }

            if (!int.TryParse(claim.Value, out int staffid))
            {
                return BadRequest("Invalid staff ID."); // ID không hợp lệ
            }

            // Kiểm tra nhân viên hiện tại
            if (staffid != nhanVien.MaNv)
            {
                return NotFound(); // Không khớp nhân viên
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(nhanVien);
                    await db.SaveChangesAsync();
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
                return RedirectToAction(nameof(Profile));
            }
            return RedirectToAction("Profile");

        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        private bool NhanVienExists(int id)
        {
            return db.NhanViens.Any(e => e.MaNv == id);
        }
    }


}
