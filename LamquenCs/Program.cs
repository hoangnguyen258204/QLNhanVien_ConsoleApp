using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LamquenCs
{
    public class Program
    {
        public class ConNguoi
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string myName, gender;
            int age, choice;
            bool isMale,isExit;
            isExit = false;
            Console.Write("Vui lòng nhập tên của bạn: ");
            myName = Console.ReadLine();
            Console.Write("Vui lòng nhập tuổi của bạn: ");
            age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Bạn có phải là giới tính nam không? y/n: ");
            isMale = Console.ReadLine().ToLower() == "y" ? true : false;
            if (isMale)
            {
                gender = "Nam";
            }
            else
            { 
                gender = "Nữ";
            }
            Console.Write("\n Thời gian bây giờ của bạn là gì (Hãy nhập số): 0-Sáng, 1-Chiều, 2-Tối: ");
            choice =Convert.ToInt32(Console.ReadLine());
            while (isExit != true)
            {
                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Chào buổi sáng!");
                        break;
                    case 1:
                        Console.WriteLine("Chào buổi chiều!");
                        break;
                    case 2:
                        Console.WriteLine("Chào buổi tối!");
                        break;
                    default:
                        Console.WriteLine("Xin chào!");
                        break;
                }
                Console.WriteLine($"Bạn là {myName}, {age} tuổi và có giới tính là {gender}.");
                Console.Write("Nhấn bất kì nút nào để tiếp tục: ");
                Console.Read();
                isExit = true;
            }
            
            int[] numbers = new int[10];
            numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            Console.Write("Số chia hết cho 2 từ 1 đến 10 là: ");
            foreach(int number in numbers)
            {
                if (number % 2 == 0)
                {
                    Console.Write($"{number}, ");
                }
            }
            
            List<string> list = new List<string>();
            list = ["Thông minh", "Sức khỏe tốt"];
            Console.Write("\nHiện bạn đang có: ");
            foreach (string item in list)
            {
                Console.Write(" | {0}", item);
            }
            Console.Write("\n Sau 30 năm chăm chỉ cày cuốc, giờ bạn có: ");
            list.Remove("Sức khỏe tốt");
            list.Add("Tiền"); list.Add("Nhà lầu"); list.Add("Xe hơi"); list.Add("Sức khỏe yếu");
            foreach (string item in list)
            {
                Console.Write(" | {0}", item);
            }
            
            string writeText = "Đây là dòng chữ lưu trên text.";
            File.WriteAllText("filetext.txt", writeText);
            string readText = File.ReadAllText("filetext.txt");
            Console.WriteLine("\n", readText);
            
            string jsonString = "{\"Name\":\"Viet\", \"Age\":16}";
            // Quá trình chuyển đổi chuỗi văn bản định dạng (Json) thành đối tượng ConNguoi
            ConNguoi connguoi = JsonSerializer.Deserialize<ConNguoi>(jsonString);
            Console.WriteLine($"Tên: {connguoi.Name}, Tuổi: {connguoi.Age}");
            // Quá trình ngược lại
            string jsonString1 = JsonSerializer.Serialize<ConNguoi>(connguoi);
            Console.WriteLine(jsonString1);

            File.WriteAllText("person.json", jsonString1);
            var jsonString2 = File.ReadAllText("person.json");
            ConNguoi connguoi1 = JsonSerializer.Deserialize<ConNguoi>(jsonString2);
            Console.WriteLine($"Tên: {connguoi1.Name}, Tuổi: {connguoi1.Age}");
            Console.ReadKey();
        }
    }
}