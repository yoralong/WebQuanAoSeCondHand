using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;
namespace Web_Bán_Quần_Áo_SecondHand.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        dbQLShopSHDataDataContext db = new dbQLShopSHDataDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
		{
            return View();
		}
        [HttpPost]
        public ActionResult Dangky(FormCollection  collection,KhachHang kh)
		{
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["NhaplaiMatkhau"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
			
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
			if (String.IsNullOrEmpty(hoten))
			{
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
			}
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại";
            }
           
            else
			{
                kh.TenKH = hoten;
                kh.MatKhau = matkhau;
                kh.TaiKhoan = tendn;
                kh.Email = email;
                kh.Sdt = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap","Nguoidung");
			}
            return this.Dangky();
        }

        public ActionResult Dangnhap()
		{
            return View();
		}

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
		{
            var tendn = collection["TenDN"];
            var mk = collection["Matkhau"];
			if (String.IsNullOrEmpty(tendn))
			{
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
			}
            else if (String.IsNullOrEmpty(mk))
			{
                ViewData["Loi2"] = "Phải nhập mật khẩu";
			}
			else
			{
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan.Trim() == tendn.Trim() && n.MatKhau.Trim() == mk.Trim());
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh.TenKH;
                    
                    return RedirectToAction("index", "ShopSecondHand");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
			}
            return View();
		}
        
    }
}