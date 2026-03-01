using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLNhanVien.Models;

namespace WebQLNhanVien.Controllers
{
    public class StaffController : Controller
    {

        private const string SESSION_KEY = "DSNhanVien";
        private List<NhanVien> TaoNhanVien()
        {
            var danhSachNhanVien = Helper.NhanVienHelper.CreateRandomEmployee();
            Session[SESSION_KEY] = danhSachNhanVien;
            return danhSachNhanVien;
        }

        private static string SinhMaNhanVien(List<NhanVien> danhSachNhanVien)
        {
            int maxId = 0;
            foreach (var nv in danhSachNhanVien)
            {
                string[] parts = nv.MaNV.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int id))
                {
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                }
            }
            return "NV-" + (maxId + 1).ToString("D4");
        }

        private List<NhanVien> LayDanhSach()
        {
            var ds = Session[SESSION_KEY] as List<NhanVien>;
            if (ds == null)
            {
                ds = TaoNhanVien();
                Session[SESSION_KEY] = ds;
            }
            return ds;
        }

        // GET: Staff
        public ActionResult Index(string keyword)
        {

            var ds = LayDanhSach();
            if (!string.IsNullOrEmpty(keyword))
            {
                ds = ds.Where(nv => (nv.HoTen != null && nv.HoTen.ToLower().Contains(keyword.ToLower()))
                    || (nv.diaChi != null && nv.diaChi.ToLower().Contains(keyword.ToLower()))
                    ).ToList();
            }
            return View(ds);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhanVien nv)
        {
            if (!ModelState.IsValid)
            {
                return View(nv);
            }
            var ds = LayDanhSach();

            nv.MaNV = SinhMaNhanVien(ds);

            ds.Add(nv);
            Session[SESSION_KEY] = ds;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var nv = LayDanhSach().FirstOrDefault(x => x.MaNV == id);
            if (nv == null)
            {
                return RedirectToAction("Index");
            }
            return View(nv);
        }

        [HttpPost]
        public ActionResult Update(NhanVien nv)
        {
            var ds = LayDanhSach();
            var old = ds.FirstOrDefault(x => x.MaNV == nv.MaNV);
            if (old != null)
            {
                old.HoTen = nv.HoTen;
                old.NgaySinh = nv.NgaySinh;
                old.soDT = nv.soDT;
                old.chucVu = nv.chucVu;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        { 
            var ds = LayDanhSach();
            var nv = ds.FirstOrDefault(x => x.MaNV == id);
            if (nv != null)
            {
                ds.Remove(nv);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Report()
        {
            return Content("Đang xây dựng");
        }
    }
}