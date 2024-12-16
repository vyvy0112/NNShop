using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NNShop.Data;

namespace NNShop.Controllers
{
	public class DanhMucHHController : Controller
	{
		private readonly NnshopContext _context;

		public DanhMucHHController(NnshopContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var danhsach = _context.LoaiHhs.ToList();
			return View(danhsach);
		}


		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Create(LoaiHh danhmuc)
		{
			if(ModelState.IsValid)
			{
				_context.LoaiHhs.Add(danhmuc);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(danhmuc);
		}


		public async Task<IActionResult> Details(int? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			
			var danhmuchh = await _context.LoaiHhs
				.FirstOrDefaultAsync(m => m.MaLoaiHh == id);
			if(danhmuchh == null)
			{
				return NotFound();
			}
			return View(danhmuchh);
		}


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.LoaiHhs.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoaiHh,TenLoaiHh")] LoaiHh danhmuc)
        {
            if (id != danhmuc.MaLoaiHh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.LoaiHhs.Update(danhmuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(danhmuc.MaLoaiHh))
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
            return View(danhmuc);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhmuc = await _context.LoaiHhs
                .FirstOrDefaultAsync(m => m.MaLoaiHh  == id);
            if (danhmuc == null)
            {
                return NotFound();
            }

            return View(danhmuc);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhmuc = await _context.LoaiHhs.FindAsync(id);
            if (danhmuc != null)
            {
                _context.LoaiHhs.Remove(danhmuc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.LoaiHhs.Any(e => e.MaLoaiHh == id);
        }
    }
}
