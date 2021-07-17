using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;
using PagedList.Mvc;
using PagedList; 
namespace Web_Bán_Quần_Áo_SecondHand.Controllers
{
    public class AdminController : Controller       
    {
        dbQLShopSHDataDataContext db = new dbQLShopSHDataDataContext();
        // GET: Admin
        public ActionResult IndexAdmin()
        {
            return View();
        }
        public ActionResult Sanpham(int? page)
        {
            // tao bien quy dinh so san pham moi tren trang
            int pageSize = 12;
            // tao bien so trang
            int pageNum = (page ?? 1);
            //lấy top sản phẩm bán chạy nhất
           
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNum, pageSize));
        }
        public ActionResult Edit()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin.Trim() == tendn.Trim() && n.PassAdmin.Trim() == matkhau.Trim());
                if (ad != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = ad;
                    Session["Taikhoandn"] = ad.Hoten;

                    return RedirectToAction("IndexAdmin", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}