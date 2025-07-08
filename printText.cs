using Spectre.Console;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleGame
{
    internal class printText
    {
        /// <summary>
        /// In văn bản ra console từ từ, hỗ trợ màu sắc từ Spectre.Console Markup
        /// và xử lý các ký tự thoát như `\[`, `\]`, `\\`.
        /// </summary>
        /// <param name="vanBanMarkup">Chuỗi văn bản có chứa Markup của Spectre.Console.</param>
        /// <param name="delayPerCharMs">Thời gian chờ giữa mỗi ký tự (miligiây).</param>
        public static void InVanBan(string vanBanMarkup, int delayPerCharMs = 40)
        {
            // Regex này sẽ chia chuỗi thành các "token" sau:
            // 1. Thẻ mở: (\[(?!/).*?\])                e.g., [green], [bold red] - KHÔNG khớp thẻ đóng
            // 2. Thẻ đóng: (\[\/.*?\])                   e.g., [/], [/green]
            // 3. Dấu backslash thoát: (\\\\)             e.g., \\
            // 4. Dấu ngoặc mở thoát: (\\\[)              e.g., \[
            // 5. Dấu ngoặc đóng thoát: (\\\])            e.g., \]
            // 6. Ký tự xuống dòng: (\n)                  e.g., \n
            // 7. Bất kỳ ký tự nào khác: (.)              (ký tự đơn lẻ)
            // Đảm bảo thứ tự ưu tiên: các thẻ và ký tự thoát đặc biệt trước, sau đó là ký tự đơn lẻ.
            var pattern = @"(\[(?!/).*?\])|(\[\/.*?\])|(\\\\)|(\\\[)|(\\\])|(\n)|(.)";
            var regex = new Regex(pattern, RegexOptions.Singleline);

            var matches = regex.Matches(vanBanMarkup);

            Style currentStyle = Style.Plain; // Bắt đầu với style mặc định (không màu)

            foreach (Match match in matches)
            {
                string value = match.Value; // Giá trị của lần khớp hiện tại

                // Kiểm tra từng nhóm bắt giữ để xác định loại token
                if (match.Groups[1].Success) // Nhóm 1: Thẻ mở (e.g., [green])
                {
                    try
                    {
                        // Trích xuất định nghĩa style từ bên trong thẻ mở (bỏ đi '[' và ']')
                        string styleDef = value.Substring(1, value.Length - 2);
                        currentStyle = Style.Parse(styleDef); // Tạo Style từ định nghĩa
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi nếu không thể parse style (ví dụ: "[invalid_color]")
                        AnsiConsole.Markup($"[red]Lỗi Style: {value} - {ex.Message}[/]");
                        currentStyle = Style.Plain; // Về style mặc định để tránh crash
                    }
                }
                else if (match.Groups[2].Success) // Nhóm 2: Thẻ đóng (e.g., [/], [/green])
                {
                    currentStyle = Style.Plain; // Về style mặc định
                }
                else if (match.Groups[3].Success) // Nhóm 3: Dấu backslash thoát (\\)
                {
                    AnsiConsole.Write(new Text("\\", currentStyle)); // In một dấu '\' thực sự
                    Thread.Sleep(delayPerCharMs);
                }
                else if (match.Groups[4].Success) // Nhóm 4: Dấu ngoặc mở thoát (\[)
                {
                    AnsiConsole.Write(new Text("[", currentStyle)); // In một dấu '[' thực sự
                    Thread.Sleep(delayPerCharMs);
                }
                else if (match.Groups[5].Success) // Nhóm 5: Dấu ngoặc đóng thoát (\])
                {
                    AnsiConsole.Write(new Text("]", currentStyle)); // In một dấu ']' thực sự
                    Thread.Sleep(delayPerCharMs);
                }
                else if (match.Groups[6].Success) // Nhóm 6: Ký tự xuống dòng (\n)
                {
                    Console.WriteLine(); // Xuống dòng thực sự
                    // Không cần Thread.Sleep ở đây vì nó là hành động tức thì
                }
                else if (match.Groups[7].Success) // Nhóm 7: Bất kỳ ký tự nào khác (ký tự đơn lẻ)
                {
                    // In ký tự đơn lẻ này với style hiện tại
                    AnsiConsole.Write(new Text(value, currentStyle));
                    Thread.Sleep(delayPerCharMs);
                }
            }
        }
    }
}
