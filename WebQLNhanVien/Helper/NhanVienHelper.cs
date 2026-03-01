using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQLNhanVien.Models;

namespace WebQLNhanVien.Helper
{
    public static class NhanVienHelper
    {
        public static List<NhanVien> CreateRandomEmployee()
        {
            List<NhanVien> danhSachNhanVien = new List<NhanVien>();
            Random rand = new Random();

            List<string> tenMau = new List<string> { "Dương Thị Nhật Lệ", "Phạm Xuân Tiến", "Tô Minh Quân", "Hà Mạnh Đức", "Trần Mình Hằng", "Nguyễn Đình Nam", "Nguyễn Thị Diệu An", "Nguyễn Cúc Mai", "Trần Đức Huy", "Nguyễn Xuân Khánh" };
            List<string> diaChiMau = new List<string> { "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Sơn La", "Quảng Ninh", "Nha Trang", "Hà Giang", "An Giang" };
            List<string> chucVuMau = new List<string> { "Nhân viên", "Trưởng phòng", "Giám đốc", "Phó giám đốc", "Thực tập sinh", "Quản Lý", "Kế Toán" };
            for (int i = 0; i < 5; i++)
            {
                NhanVien nv = new NhanVien
                {
                    MaNV = "NV-" + (i + 1).ToString("D4"),
                    HoTen = tenMau[rand.Next(tenMau.Count)],
                    NgaySinh = DateTime.Now.AddYears(-rand.Next(20, 50)).AddDays(rand.Next(0, 365)),
                    soDT = "09" + rand.Next(10000000, 99999999).ToString(),
                    diaChi = diaChiMau[rand.Next(diaChiMau.Count)],
                    chucVu = chucVuMau[rand.Next(chucVuMau.Count)],
                    namCongTac = rand.Next(1, 50)
                };
                tenMau.Remove(nv.HoTen);

                if (nv.chucVu == "Giám đốc" || nv.chucVu == "Phó giám đốc")
                {
                    chucVuMau.Remove(nv.chucVu);
                }

                danhSachNhanVien.Add(nv);
            }

            return danhSachNhanVien;
        }
    }
}