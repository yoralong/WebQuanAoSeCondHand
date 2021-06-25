using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;

namespace Web_Bán_Quần_Áo_SecondHand.Controllers
{
    public class ShopSecondHandController : Controller
    {
        // GET: ShopSecondHand
        dbQLShopSHDataContext data = new dbQLShopSHDataContext();
       
        private List<SanPham> laysanpham (int count)
		{
            return data.SanPhams.OrderByDescending(a => a.NgayNhap).Take(count).ToList();
		}
        public ActionResult Index()
        {
            var sanphammoi = laysanpham(12);
            return View(sanphammoi);
        }

        public ActionResult BoSuuTap()
		{
            var suutap = from cd in data.LoaiSPs select cd;
            return PartialView(suutap);
		}


    }
}