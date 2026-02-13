using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using QLNhanVien_ConsoleApp.Models;

namespace QLNhanVien_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            int choice;
            List<NhanVien> danhSachNhanVien = LoadDuLieu();
            do
            {

                Menu();
                // Đọc lựa chọn của người dùng
                choice = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

                switch (choice)
                {
                    case 1:
                        HienThi(danhSachNhanVien);
                        break;
                    case 2:
                        ThemMoi(danhSachNhanVien);
                        break;
                    case 3:
                        SuaThongTin(danhSachNhanVien);
                        break;
                    case 4:
                        XoaNhanVien(danhSachNhanVien);
                        break;
                    case 5:
                        TimKiem(danhSachNhanVien);
                        break;
                    case 6:
                        LuuRaFile(danhSachNhanVien);
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            } while (choice != 0);
            
        }

        // Hàm lấy danh sách nhân viên từ file (định dạng JSON)
        private static List<NhanVien> LoadDuLieu()
        {
            string filePath = "danh_sach_nhan_vien.json";
            List<NhanVien> danhSachNhanVien = new List<NhanVien>();
            try
            {
                if (File.Exists(filePath))
                {
                    var jsonString = File.ReadAllText(filePath);
                    danhSachNhanVien = JsonSerializer.Deserialize<List<NhanVien>>(jsonString);
                }
                else
                {
                    danhSachNhanVien = CreateRandomEmployee();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file: {ex.Message}");
            }
            return danhSachNhanVien;
        }

        // Hàm lưu danh sách nhân viên ra file (định dạng JSON)
        private static void LuuRaFile(List<NhanVien> danhSachNhanVien)
        {
            Console.Clear();
            Console.WriteLine("Bạn đã chọn \"Lưu ra file\"");
            string filePath = "danh_sach_nhan_vien.json";
            try
            {
                var jsonString = JsonSerializer.Serialize(danhSachNhanVien);
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine($"Đã lưu danh sách nhân viên ra file thành công tại: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu file: {ex.Message}");
            }
            ESCtoMenu();
        }


        // Hàm tìm kiếm theo từ khóa
        private static void TimKiem(List<NhanVien> danhSachNhanVien)
        {
            Console.Clear();
            Console.WriteLine("Bạn đã chọn \"Tìm kiếm\"");
            Console.Write("Nhập từ khóa tìm kiếm: ");
            string keyword = Console.ReadLine().ToLower();
            List<NhanVien> ketqua = new List<NhanVien>();
            if (danhSachNhanVien != null && danhSachNhanVien.Count > 0)
            {
                foreach (NhanVien nv in danhSachNhanVien)
                {
                    if (nv.HoTen.ToLower().Contains(keyword) || nv.diaChi.ToLower().Contains(keyword))
                    {
                        ketqua.Add(nv);
                    }
                }

                if (ketqua.Count > 0)
                {
                    Console.WriteLine("---Danh sách nhân viên chứa từ khóa tìm kiếm---");
                    foreach (var nv in ketqua)
                    {
                        Console.WriteLine($"\n|Mã NV: {nv.MaNV} | Họ Tên: {nv.HoTen} | Ngày sinh: {nv.NgaySinh:dd/MM/yyyy} | SĐT: {nv.soDT} | Địa chỉ: {nv.diaChi} | Chức vụ: {nv.chucVu} | Số năm công tác: {nv.namCongTac} |");
                    }
                }
                else
                {
                    Console.WriteLine("Không tìm thấy nhân viên nào chứa từ khóa đã nhập!");
                }
            }
            else 
            {
                Console.WriteLine("Danh sách nhân viên trống!");
            }
            ESCtoMenu();
        }

        // Hàm xóa nhân viên
        private static void XoaNhanVien(List<NhanVien> danhSachNhanVien)
        {
            Console.Clear();
            Console.WriteLine("Bạn đã chọn \"Xóa nhân viên\"");
            Console.Write("Nhập mã nhân viên cần xóa: ");
            string maNV = Console.ReadLine();
            NhanVien nv = TimKiemTheoMa(danhSachNhanVien, maNV);
            if (nv != null)
            {
                Console.WriteLine("--- Thông tin nhân viên ---");
                Console.WriteLine($"1. Họ tên:          {nv.HoTen}");
                Console.WriteLine($"2. Ngày sinh:       {nv.NgaySinh:dd/MM/yyyy}");
                Console.WriteLine($"3. Số điện thoại:   {nv.soDT}");
                Console.WriteLine($"4. Địa chỉ:         {nv.diaChi}");
                Console.WriteLine($"5. Chức vụ:         {nv.chucVu}");
                Console.WriteLine($"6. Số năm công tác: {nv.namCongTac}");
                Console.Write($"Bạn có chắc chắn muốn xóa nhân viên {nv.HoTen} không? (Y/N): ");
                var confirm = Console.ReadLine();
                if (confirm.ToUpper() == "Y")
                {
                    danhSachNhanVien.Remove(nv);
                    Console.WriteLine("Đã xóa nhân viên thành công!");
                }
                else
                {
                    Console.WriteLine("Hủy xóa nhân viên.");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy nhân viên với mã đã nhập!");
            }
            ESCtoMenu();
        }

        // Hàm sửa thông tin nhân viên
        private static void SuaThongTin(List<NhanVien> danhSachNhanVien)
        {
            Console.Clear();
            Console.WriteLine("Bạn đã chọn \"Sửa thông tin nhân viên\"");
            Console.Write("Nhập mã nhân viên cần sửa: ");
            string maNV = Console.ReadLine();
            NhanVien nv = TimKiemTheoMa(danhSachNhanVien, maNV);
            NhanVien oldData = new NhanVien
            {
                HoTen = nv.HoTen,
                NgaySinh = nv.NgaySinh,
                soDT = nv.soDT,
                diaChi = nv.diaChi,
                chucVu = nv.chucVu,
                namCongTac = nv.namCongTac
            };
            if (nv != null)
            {
                Console.WriteLine("--- Thông tin nhân viên ---");
                Console.WriteLine($"1. Họ tên:          {nv.HoTen}");
                Console.WriteLine($"2. Ngày sinh:       {nv.NgaySinh:dd/MM/yyyy}");
                Console.WriteLine($"3. Số điện thoại:   {nv.soDT}");
                Console.WriteLine($"4. Địa chỉ:         {nv.diaChi}");
                Console.WriteLine($"5. Chức vụ:         {nv.chucVu}");
                Console.WriteLine($"6. Số năm công tác: {nv.namCongTac}");
                Console.Write($"---> Hãy chọn thông tin cần sửa theo số: ");
                int choice = choice = int.TryParse(Console.ReadLine(), out int result) ? result : -1;
                switch (choice)
                {
                    case 1:
                        Console.Write("Nhập họ tên mới: ");
                        nv.HoTen = Console.ReadLine();
                        break;
                    case 2:
                        Console.Write("Nhập ngày sinh mới (dd/MM/yyyy): ");
                        string dateInput = Console.ReadLine();
                        if (DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
                        {
                            nv.NgaySinh = ngaySinh;
                        }
                        else
                        {
                            Console.WriteLine("\nĐịnh dạng ngày sinh không hợp lệ! Quay trở về Menu");
                            return;
                        }
                        break;
                    case 3:
                        Console.Write("Nhập số điện thoại mới: ");
                        nv.soDT = Console.ReadLine();
                        break;
                    case 4:
                        Console.Write("Nhập địa chỉ mới: ");
                        nv.diaChi = Console.ReadLine();
                        break;
                    case 5:
                        Console.Write("Nhập chức vụ mới: ");
                        nv.chucVu = Console.ReadLine();
                        break;
                    case 6:
                        Console.Write("Nhập số năm công tác mới: ");
                        nv.namCongTac = int.Parse(Console.ReadLine());
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Quay trở về Menu");
                        return;
                }
                Console.Write("Bạn có muốn lưu thay đổi? (Y/N): ");
                var confirm = Console.ReadLine();
                if (confirm.ToUpper() == "Y")
                {
                    Console.WriteLine("Đã lưu thành công!");
                }
                else
                {
                    // Nếu người dùng không xác nhận, khôi phục lại dữ liệu cũ
                    nv.HoTen = oldData.HoTen;
                    nv.NgaySinh = oldData.NgaySinh;
                    nv.soDT = oldData.soDT;
                    nv.diaChi = oldData.diaChi;
                    nv.chucVu = oldData.chucVu;
                    nv.namCongTac = oldData.namCongTac;
                    Console.WriteLine("Đã hủy thay đổi!");
                }

                ESCtoMenu();

            }
            else 
            { 
                Console.WriteLine("Không tìm thấy nhân viên với mã đã nhập!");
                ESCtoMenu();
            }

        }


        // Hàm thêm mới nhân viên
        private static void ThemMoi(List<NhanVien> danhSachNhanVien)
        {   
            NhanVien nv = new NhanVien();
            Console.Clear();
            Console.WriteLine("Bạn đã chọn \"Thêm mới nhân viên\"");
            nv.MaNV = SinhMaNhanVien(danhSachNhanVien);
            Console.Write("Nhập họ tên*: ");
            nv.HoTen = Console.ReadLine();
            Console.Write("Nhập ngày sinh (dd/MM/yyyy)*: ");
            string dateInput = Console.ReadLine();
            if (DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
            {
                nv.NgaySinh = ngaySinh;
            }
            else
            {
                Console.WriteLine("\nĐịnh dạng ngày sinh không hợp lệ! Quay trở về Menu");
                ESCtoMenu();
                return;
            }
            Console.Write("Nhập số điện thoại: ");
            nv.soDT = Console.ReadLine();
            Console.Write("Nhập địa chỉ: ");
            nv.diaChi = Console.ReadLine();
            Console.Write("Nhập chức vụ: ");
            nv.chucVu = Console.ReadLine();
            Console.Write("Nhập số năm công tác*: ");
            nv.namCongTac = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

            var isDuplicate = KiemTraTrungLap(danhSachNhanVien, nv.HoTen, nv.NgaySinh);
            if (string.IsNullOrWhiteSpace(nv.HoTen) || nv.namCongTac < 0)
            {
                Console.WriteLine("Vui lòng nhập đầy đủ thông tin bắt buộc!");

            } else if (isDuplicate != null)
            {
                Console.WriteLine("Nhân viên đã tồn tại trong danh sách!");
                
            } else
            {
                danhSachNhanVien.Add(nv);
                Console.WriteLine("Đã thêm nhân viên thành công!");
            }
            ESCtoMenu();
        }

        // Hàm hiển thị danh sách nhân viên
        private static void HienThi(List<NhanVien> danhSachNhanVien)
        {
                Console.Clear();
                Console.WriteLine("Bạn đã chọn \"Hiển thị danh sách nhân viên\"");
                Console.WriteLine("Danh sách nhân viên:");
                foreach (var nv in danhSachNhanVien)
                {
                    Console.WriteLine($"\n|Mã NV: {nv.MaNV} | Họ Tên: {nv.HoTen} | Ngày sinh: {nv.NgaySinh:dd/MM/yyyy} | SĐT: {nv.soDT} | Địa chỉ: {nv.diaChi} | Chức vụ: {nv.chucVu} | Số năm công tác: {nv.namCongTac} |");
                }
                ESCtoMenu();
        }

        // Hàm kiểm tra nhân viên theo tên và ngày sinh
        private static NhanVien KiemTraTrungLap(List<NhanVien> danhSachNhanVien, string tenNV, DateTime ngaySinh)
        {
            NhanVien ketqua = null;
            if (danhSachNhanVien != null && danhSachNhanVien.Count > 0)
            {
                foreach (NhanVien nv in danhSachNhanVien)
                {
                    if (nv.HoTen == tenNV && nv.NgaySinh.ToShortDateString() == ngaySinh.ToShortDateString())
                    {
                        ketqua = nv;
                        break;
                    }
                }
            }
            return ketqua;
        }

        // Hàm tìm kiếm nhân viên theo mã
        private static NhanVien TimKiemTheoMa(List<NhanVien> danhSachNhanVien, string maNV)
        {
            NhanVien ketqua = null;
            if (danhSachNhanVien != null && danhSachNhanVien.Count > 0)
            {
                foreach (NhanVien nv in danhSachNhanVien)
                {
                    if (nv.MaNV == maNV)
                    {
                        ketqua = nv;
                        break;
                    }
                }
            }
            return ketqua;
        }

        // Hàm sinh mã nhân viên tự động
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

        // Hàm quay lại menu khi nhấn ESC
        private static void ESCtoMenu()
        {
            while (true)
            {
                Console.WriteLine("\nNhấn ESC để quay lại menu...");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("\nPhím bấm không hợp lệ");
                }
            }
        }

        // Hàm hiển thị menu
        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("\n-----------------MENU-----------------");
            Console.WriteLine("1 - Hiển thị danh sách nhân viên");
            Console.WriteLine("2 - Thêm mới nhân viên");
            Console.WriteLine("3 - Sửa thông tin nhân viên");
            Console.WriteLine("4 - Xóa nhân viên");
            Console.WriteLine("5 - Tìm kiếm");
            Console.WriteLine("6 - Lưu ra file");
            Console.WriteLine("0 - Thoát");
            Console.Write("Vui lòng lựa chọn: ");
        }

        // hàm tạo danh sách nhân viên ngẫu nhiên
        static List<NhanVien> CreateRandomEmployee()
        {
            List<NhanVien> danhSachNhanVien = new List<NhanVien>();
            Random rand = new Random();

            List<string> tenMau = new List<string>{ "Dương Thị Nhật Lệ", "Phạm Xuân Tiến", "Tô Minh Quân", "Hà Mạnh Đức", "Trần Mình Hằng", "Nguyễn Đình Nam", "Nguyễn Thị Diệu An", "Nguyễn Cúc Mai", "Trần Đức Huy", "Nguyễn Xuân Khánh"};
            List<string> diaChiMau =  new List<string>{ "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Sơn La", "Quảng Ninh", "Nha Trang", "Hà Giang", "An Giang"};
            List<string> chucVuMau =  new List<string>{ "Nhân viên", "Trưởng phòng", "Giám đốc", "Phó giám đốc", "Thực tập sinh", "Quản Lý", "Kế Toán"};
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

                if(nv.chucVu == "Giám đốc" || nv.chucVu == "Phó giám đốc")
                {
                    chucVuMau.Remove(nv.chucVu);
                }

                danhSachNhanVien.Add(nv);
            }

            return danhSachNhanVien;
        }
    }
}
