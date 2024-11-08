using Microsoft.Ajax.Utilities;
using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class GiohangController : Controller
    {
        private readonly QLBansachEntities _context = new QLBansachEntities();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lsGiohang = Session["Giohang"] as List<Giohang>;

            if (lsGiohang == null)
            {
                lsGiohang = new List<Giohang>();
                Session["Giohang"] = lsGiohang;
            }
            return lsGiohang;
        }


        // Tong so luong
        private int TongSoLuong()
        {
            int iTongSL = 0;
            List<Giohang> lsGiohang = Session["GioHang"] as List<Giohang>;
            if (lsGiohang != null)
                iTongSL = lsGiohang.Sum(n => n.iSoluong);
            return iTongSL;

        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lsGiohang = Session["GioHang"] as List<Giohang>;
            if (lsGiohang != null)
                iTongTien = lsGiohang.Sum(n => n.dThanhtien);
            return iTongTien;
        }

        // Them hang vao gio
        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            List<Giohang> lsGiohang = Laygiohang();

            Giohang sanpham = lsGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lsGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        // Xay dung trang gio hang
        public ActionResult GioHang()
        {
            List<Giohang> lsGiohang = Laygiohang();
            if (lsGiohang.Count == 0)
                return RedirectToAction("Index", "BookStore");

            ViewBag.Tongsl = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lsGiohang);
        }


        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lsGiohang = Laygiohang();
            Giohang sanpham = lsGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                lsGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("GioHang");
            }

            if (lsGiohang.Count == 0)
                return RedirectToAction("Index", "BookStore");
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lsGiohang = Laygiohang();
            Giohang sp = lsGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sp != null)
                sp.iSoluong = int.Parse(f["txtSoluong"].ToString());

            return RedirectToAction("Giohang");
        }

        public ActionResult XoaALL()
        {
            List<Giohang> lsGiohang = Laygiohang();
            lsGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }

            if (Session["Giohang"] == null)
                return RedirectToAction("Index", "BookStore");

            List<Giohang> lsGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lsGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        { 
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now; ;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            _context.DONDATHANGs.Add(ddh);
            _context.SaveChanges();

            foreach(var i in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = i.iMasach;
                ctdh.Dongia = (decimal)i.dDongia;
                _context.CHITIETDONTHANGs.Add(ctdh);

            }
            
            _context.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");

           }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}