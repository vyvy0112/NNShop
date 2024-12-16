using Microsoft.AspNetCore.Mvc;
using NNShop.Data;
using NNShop.ViewModels;

namespace NNShop.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly NnshopContext db;

        public MenuLoaiViewComponent(NnshopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.LoaiHhs.Select(lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoaiHh,
                TenLoai = lo.TenLoaiHh,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);

            return View(data);
        }
    }
}
