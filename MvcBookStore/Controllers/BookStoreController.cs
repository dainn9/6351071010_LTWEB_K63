using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
           QLBansachEntities data = new QLBansachEntities();
        // GET: BookStore

        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).ToList();
        }
        public ActionResult Index()
        {
            var sachmoi = Laysachmoi(5);
            return View(sachmoi);
        }

        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }

        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD ==  id select s;
            return View(sach);
        }

        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s; 
            return View(sach);
        }

        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}