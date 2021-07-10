using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Bán_Quần_Áo_SecondHand.Models;

namespace Web_Bán_Quần_Áo_SecondHand.Controllers
{
    public class GiohangController : Controller
    {
        // Tao doi tuong data chua du lieu
        dbQLShopSHDataDataContext data = new dbQLShopSHDataDataContext();
        // Lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang == null)
            {
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }

        //Them gio hang
        public ActionResult ThemGiohang (int masp , string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> listGiohang = Laygiohang();
            //kiem tra san pham nay ton tai trong Session chua ?
            Giohang sanpham = listGiohang.Find(n => n.MaSP == masp);
            if(sanpham==null)
            {
                sanpham = new Giohang(masp);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.Soluong++;
                return Redirect(strURL);
            }
        }

        //Tổng số lượng

        private int TongSoLuong()
        {
            int tongsoluong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang!=null){
                tongsoluong = listGiohang.Sum(p => p.Soluong);
            }
            return tongsoluong;
        }

        //Tong Tien

        private double TongTien()
        {
            double tongtien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                tongtien = listGiohang.Sum(p => p.ThanhTien);
            }
            return tongtien;
        }

        // Xay dung trang gio hang
        public ActionResult GioHang()
        {
            List<Giohang> listGiohang = Laygiohang();
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "ShopSecondHand");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(listGiohang);
        }
        
        //Tạo partial view de hiển thị thông tin giỏ hàng
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            return PartialView();
        }

        //xoa gio hang
        public ActionResult XoaGiohang(int masp)
        {
            //Lay gio hang tu Session
            List<Giohang> listGiohang = Laygiohang();
            // Kiem tra san pham co trong Sesssion chua?
            Giohang sanpham = listGiohang.SingleOrDefault(p => p.MaSP == masp);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.MaSP == masp);
                return RedirectToAction("GioHang");
            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "ShopSecondHand");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhapGiohang(int masp, FormCollection f)
        {
            List<Giohang> listGiohang = Laygiohang();
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.MaSP == masp);
            if (sanpham != null)
            {
                sanpham.Soluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult XoatatcaGiohang()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("index", "ShopSecondHand");
        }
    }
}