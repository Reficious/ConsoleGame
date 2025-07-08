using Spectre;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO; //thư viện đọc, ghi
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    internal class Functions
    {
        public static void SaveGame(string viTriLuuGame, List<int> stats, string tentaikhoan)
        {
            try 
            {
                using (StreamWriter writer = new StreamWriter(viTriLuuGame))
                {
                    writer.WriteLine($"0: {tentaikhoan}"); // Ghi tên tài khoản ở dòng đầu
                    for (int i = 0; i < stats.Count; i++)
                    {
                        writer.WriteLine($"{i + 1}: {stats[i]}"); // Ghi các stat từ index 1
                    }
                }
                AnsiConsole.MarkupLine("Lưu game [green]thành công[/]");
                Console.ReadKey();

            }catch (Exception e) 
            {
                AnsiConsole.MarkupLine($"[red]Lỗi:[/] không lưu game thành công!");
                Console.WriteLine($"Thông báo lỗi: {e}");
            }
        }

        public static void LoadGame(string viTriLuuGame,ref List<int> stats,ref string tentaikhoan)
        {
            try
            {
                stats.Clear();
                using (StreamReader reader = new StreamReader(viTriLuuGame))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // lấy dữ liệu -> "0: Thạch Sùng", Split kí tự ": " -> "0" (parts[0]) & "Thạch Sùng" (parts[1])
                        string[] parts = line.Split(new string[] { ": " }, StringSplitOptions.None);
                        if (parts.Length == 2) // Đảm bảo dòng có định dạng đúng, tách thành 2 phần
                        {
                            int index = int.Parse(parts[0]); // Lấy chỉ số, mặc định cho parts[0] là chỉ số, stt.
                            if (index == 0)
                            {
                                tentaikhoan = parts[1]; // Dòng đầu là tên tài khoản
                            }
                            else
                            {
                                stats.Add(int.Parse(parts[1])); // Các dòng sau là chỉ số
                            }
                        }
                    }
                }

                AnsiConsole.MarkupLine("Tải dữ liệu game [green]thành công![/]");
                Console.ReadKey();
                printText.InVanBan("Chào mừng bạn trở lại hành trình! \n", 0);
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi:[/] không lưu game thành công!");
                Console.WriteLine($"Thông báo lỗi: {e}");
            }
        }
    }
}
