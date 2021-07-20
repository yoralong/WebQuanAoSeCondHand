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
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
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
        public ActionResult Sanpham(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNum, pageSize));
            }

        }
        public ActionResult QLTKKhachhang(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.KhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNum, pageSize));
            }

        }
        public ActionResult vanchuyen(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.CongTy_VanChuyens.ToList().OrderBy(n => n.MaCT).ToPagedList(pageNum, pageSize));
            }

        }


        public ActionResult tkadmin(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.Admins.ToList().OrderBy(n => n.UserAdmin).ToPagedList(pageNum, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sanPham.MaSP;
                if (sanPham == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sanPham);
            }

        }
        [HttpPost, ActionName("Delete")]
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

            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.MaLoai), "Maloai", "TenLoai");
                ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.MaTH), "MaTH", "TenTH");
                return View();
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase fileupload)
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
                if (ModelState.IsValid)
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
                    sanPham.Image = "/Content/Images/" + fileName;
                    db.SanPhams.InsertOnSubmit(sanPham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sanpham", "Admin");
            }
        }


        public ActionResult Details(int id)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sanPham.MaSP;
                if (sanPham == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sanPham);
            }

        }
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MaLoai = new SelectList(db.LoaiSPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanPham.MaLoai);
                ViewBag.MaTH = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", sanPham.MaTH);
                return View(sanPham);
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult XacnhanEdit(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
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
                        {
                            ViewBag.Thongbao = "Hình này đã tồn tại";

                        }
                        else
                            //úp hình lên server
                            fileUpload.SaveAs(path);
                        sanPham.Image = "/Content/Images/" + fileName;
                        UpdateModel(sanPham);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("SanPham", "Admin");
                }


            }
        }


        //thuong hiệu
        public ActionResult thuonghieu(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.ThuongHieus.ToList().OrderBy(n => n.MaTH).ToPagedList(pageNum, pageSize));
            }

        }
        [HttpGet]
        public ActionResult CreateThuonghieu()
        {

            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateThuonghieu(ThuongHieu thuonghieu)
        {

            if (ModelState.IsValid)
            {

                db.ThuongHieus.InsertOnSubmit(thuonghieu);
                db.SubmitChanges();
            }
            return RedirectToAction("thuonghieu", "Admin");
        }
        public ActionResult SuaThuonghieu(string id)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ThuongHieu thuonghieu = db.ThuongHieus.SingleOrDefault(n => n.MaTH.Trim() == id.Trim());
                ViewBag.MaTH = thuonghieu.MaTH;
                if (thuonghieu == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(thuonghieu);
            }
        }
        [HttpPost, ActionName("SuaThuonghieu")]
        public ActionResult XacnhansuaTH(string id)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {

                ThuongHieu thuonghieu = db.ThuongHieus.SingleOrDefault(n => n.MaTH.Trim() == id.Trim());
                UpdateModel(thuonghieu);
                db.SubmitChanges();
                return RedirectToAction("thuonghieu", "Admin");
            }
        }
        [HttpGet]
        public ActionResult DeleteThuonghieu(string id)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ThuongHieu thuonghieu = db.ThuongHieus.SingleOrDefault(n => n.MaTH.Trim() == id.Trim());
                ViewBag.MaTH = thuonghieu.MaTH;
                if (thuonghieu == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(thuonghieu);
            }

        }
        [HttpPost, ActionName("DeleteThuonghieu")]
        public ActionResult xoaThuonghieu(string id)
        {
            ThuongHieu thuonghieu = db.ThuongHieus.SingleOrDefault(n => n.MaTH.Trim() == id.Trim());
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ThuongHieus.DeleteOnSubmit(thuonghieu);
            db.SubmitChanges();
            return RedirectToAction("thuonghieu", "Admin");
        }
        //CTHD
        public ActionResult CTHD(int? page)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                // tao bien quy dinh so san pham moi tren trang
                int pageSize = 6;
                // tao bien so trang
                int pageNum = (page ?? 1);
                //lấy top sản phẩm bán chạy nhất

                return View(db.CTHDs.ToList().OrderBy(n => n.MaHD).ToPagedList(pageNum, pageSize));
            }
        }
        
    }
}

