using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Bán_Quần_Áo_SecondHand.Models
{
    public class Giohang
    {
        dbQLShopSHDataDataContext data = new dbQLShopSHDataDataContext();
        public int MaSP { set; get; }
        public string TenSP { set; get; }
        public string Image { set; get; }
        public double DonGia { set; get; }
        public int Soluong { set; get; }
        public double ThanhTien {
            get { return Soluong * DonGia; }
        }

        public Giohang (int masp)
        {
            this.MaSP = masp;
            SanPham sanpham = data.SanPhams.Single(n => n.MaSP == masp);
            TenSP = sanpham.TenSP;
            Image = sanpham.Image;
            DonGia = double.Parse(sanpham.DonGia.ToString());
            Soluong = 1;
        }
    }
}