using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;
using PagedList;
using PagedList.Mvc;
namespace Web_Bán_Quần_Áo_SecondHand.Controllers
{
    public class ShopSecondHandController : Controller
    {
        // GET: ShopSecondHand
        dbQLShopSHDataDataContext data = new dbQLShopSHDataDataContext();
       
        private List<SanPham> laysanpham (int count)
		{
            return data.SanPhams.OrderByDescending(a => a.NgayNhap).Take(count).ToList();
            //chao
		}
        public ActionResult SPTheoMaLoai(string id)
        {
            var sanphamtheoloai = from cd in data.SanPhams where cd.MaLoai.Trim() == id.Trim() select cd;
            return View(sanphamtheoloai);
        }
        public ActionResult Index(int ? page)
        {
            // tao bien quy dinh so san pham moi tren trang
            int pageSize = 12;
            // tao bien so trang
            int pageNum = (page ?? 1);
            //lấy top sản phẩm bán chạy nhất
            var sanphammoi = laysanpham(36);
            return View(sanphammoi.ToPagedList(pageNum,pageSize));
        }

        public ActionResult BoSuuTap()
		{
            var suutap = from cd in data.LoaiSPs select cd;
            return PartialView(suutap);
		}
        public ActionResult Details(int id)
        {
            var details = from s in data.SanPhams
                          where s.MaSP == id
                          select s;
            return View(details.Single());
        }
        public ActionResult aboutus()
        {
            return View();
        }
        public ActionResult huongdanmuahang()
        {
            return View();
        }
        public ActionResult baohanh()
        {
            return View();
        }
        public  ActionResult index1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string keyword)
        {

            var all = data.SanPhams.Where(x => x.TenSP.Contains(keyword));

            return View(all);
        }
    }
}