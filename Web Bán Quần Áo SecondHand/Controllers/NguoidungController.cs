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
            var diachi = collection["Diachi"];
			
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            KhachHang tempt = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan.Trim() == tendn.Trim());
            
             if (tempt != null)
            {
                ViewData["Loi2"] = "Username đã tồn tại";
            }
            if (matkhau != nhaplaimatkhau)
            {
                ViewData["Loi1"] = "Nhập lại mật khẩu không đúng";
            }
            else
			{   
                //so sánh ngày hiện tại và ngày sinh
                if(DateTime.Compare(DateTime.Now, DateTime.Parse(collection["Ngaysinh"]))==-1)
                {
                    ViewData["Loi3"] = "Ngày sinh không được lớn hơn hiện tại";
                    
                }
                kh.TenKH = hoten;
                kh.MatKhau = matkhau;
                kh.TaiKhoan = tendn;
                kh.Email = email;
                kh.Sdt = dienthoai;
                kh.DiaChi = diachi;
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
                    Session["Taikhoan"] = kh;
                    Session["Taikhoandn"] = kh.TenKH;
                    
                    return RedirectToAction("GioHang", "Giohang");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
			}
            return View();
		}
        public ActionResult Logout()
        {

            Session["Taikhoan"] = null;
            Session["Taikhoandn"] = null;

            return RedirectToAction("Index", "ShopSecondHand");
        }
    }
}