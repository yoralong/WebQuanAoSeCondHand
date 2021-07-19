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
            int pageSize = 7;
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
            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.MaLoai), "Maloai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.MaTH), "MaTH", "TenTH");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SanPham sanPham,HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.MaLoai), "Maloai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.MaTH), "MaTH", "TenTH");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sanPham.Image ="~/Content/Images/"+ fileName;
                    db.SanPhams.InsertOnSubmit(sanPham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham","Admin");
            }          
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Lay ra doi tuong sach theo ma
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masach = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", sanpham.MaTH);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SanPham sanpham, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            SanPham dbUpdate = db.SanPhams.FirstOrDefault(p => p.MaSP == sanpham.MaSP);
            ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", sanpham.MaTH);
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanpham.Image = "~/Content/Images/" + fileName;
                    //Luu vao CSDL       
                    UpdateModel(dbUpdate);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham","Admin");
            }
        }
    }
}