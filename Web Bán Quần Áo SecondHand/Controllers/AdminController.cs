using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;
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
        [HttpGet]
        public ActionResult Delete(int id)
        {
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SanPham sanPham,HttpPostedFileBase fileupload)
        {
            var fileName = Path.GetFileName(fileupload.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
            if(System.IO.File.Exists(path))
            {
                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            }
            else
            {
                fileupload.SaveAs(path);
            }
            
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
        public ActionResult Details(int id)
        {
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanPham.MaSP;
            if(sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);        
        }
    }
}