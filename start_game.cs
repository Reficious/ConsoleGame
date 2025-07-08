using Spectre;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel.Design;
using System.Xml.Linq;

namespace ConsoleGame
{
    internal class start_game
    {
        static void Main(string[] args)
        {
                                // Đưa vào địa chỉ cụ thể nơi lưu game, ví dụ D:\Backups\.... đưa cụ thể vào folder này, để lưu và load file save.
            const string path = @"";

            bool run = true;
            bool menu = true;
            bool play = false;
            bool rules = false;
            bool options = false;
            bool battle = false;
            bool standing = true;
            bool speak = false;
            bool key = true;
            bool shop = false;
            bool boss = false;

            string name = "DefaultPlayer";
            int hp = 100;
            int hpMax = 100;
            int mp = 20;
            int mpMax = 20;
            int atk = 5;

            int potion = 0;
            int elixer = 0;
            int gold = 0;
            int x = 0;
            int y = 0;

            int delayPerCharMs = 50;

            string[,] map =
            {   //x = 0        x = 1        x = 2       x = 3        x = 4        x = 5       x = 6
                {"đồng cỏ",   "đồng cỏ",   "đồng cỏ",  "đồng cỏ",   "đồng cỏ",   "núi",      "hang động"},  //y = 0
                {"khu rừng",  "khu rừng",  "khu rừng", "khu rừng",  "khu rừng",  "đồi",      "núi",},       //y = 1
                {"khu rừng",  "cánh đồng", "cầu",      "đồng bằng", "đồi",       "khu rừng", "đồi"},        //y = 2
                {"đồng bằng", "cửa hàng",  "tháp",     "major",     "đồng bằng", "đồi",      "núi"},        //y = 3
                {"đồng cỏ",   "đồng cỏ",   "đồng cỏ",  "đồng cỏ",   "đồng cỏ",   "núi",      "hang động"},  //y = 4
            };

            Dictionary<string, Locations> biom = new Dictionary<string, Locations> 
            {
                {"đồng cỏ", new Locations {Name = "ĐỒNG CỎ", CoQuaiVat = true}},
                {"núi", new Locations {Name = "NÚI", CoQuaiVat = true}},
                {"hang động", new Locations {Name = "HANG ĐỘNG", CoQuaiVat = true}},
                {"khu rừng", new Locations {Name = "KHU RỪNG", CoQuaiVat = true}},
                {"đồi", new Locations {Name = "ĐỒI", CoQuaiVat = true}},
                {"cánh đồng", new Locations {Name = "CÁNH ĐỒNG", CoQuaiVat = true}},
                {"cầu", new Locations {Name = "CẦU", CoQuaiVat = true}},
                {"đồng bằng", new Locations {Name = "ĐỒNG BẰNG", CoQuaiVat = true}},
                {"tháp", new Locations {Name = "THÁP", CoQuaiVat = true}},
                {"major", new Locations {Name = "MAJOR", CoQuaiVat = false}},
                {"cửa hàng", new Locations {Name = "CỬA HÀNG", CoQuaiVat = false}}
            };

            int y_len = map.GetLength(0) -1;
            int x_len = map.GetLength(1) -1;

            string[] soquaivat = {"goblin", "orc", "slime"};

            Dictionary<string, Enemy> quaivat = new Dictionary<string, Enemy>
            {
                {"goblin", new Enemy {Name = "GOBLIN", Attack = 5, Hp = 20, HpMax = 20, dropGold = 5 } },
                {"orc", new Enemy {Name = "ORC", Attack = 20, Hp = 75, HpMax = 75, dropGold = 15 } },
                {"slime", new Enemy {Name = "SLIME", Attack = 3, Hp = 5, HpMax = 5, dropGold = 2 } },
                {"dragon", new Enemy {Name = "RỒNG SIÊU CẤP", Attack = 50, Hp = 1000, HpMax = 1000, dropGold = 99999 } }
            };

            List<int> stats = new List<int>();

            Random rdn = new Random();

            // Khởi tạo stats ban đầu
            void UpdateStats()
            {
                stats.Clear();
                stats.Add(hp);
                stats.Add(hpMax);
                stats.Add(mp);
                stats.Add(mpMax);
                stats.Add(atk);
                stats.Add(potion);
                stats.Add(elixer);
                stats.Add(gold);
                stats.Add(x);
                stats.Add(y);
                stats.Add(delayPerCharMs);
            }

            // Gán giá trị từ stats vào các biến
            void LoadStatsFromList()
            {
                if (stats.Count >= 11) // Đảm bảo stats có đủ phần tử
                {
                    hp = stats[0];
                    hpMax = stats[1];
                    mp = stats[2];
                    mpMax = stats[3];
                    atk = stats[4];
                    potion = stats[5];
                    elixer = stats[6];
                    gold = stats[7];
                    x = stats[8];
                    y = stats[9];
                    delayPerCharMs = stats[10];
                }
            }

            void Heal(int amount)
            {
                if (hp <= hpMax)
                {
                    hp += amount;
                } else
                {
                    hp = hpMax;
                }
                Console.WriteLine($"Sử dụng bình máu thành công! Bạn hồi được {amount} máu.");
            }

            void Major()
            {
                while(speak)
                {
                    Console.Clear();
                    if (atk <= 10)
                    {
                        Console.WriteLine($"Chàng trai trẻ, bạn chưa đủ mạnh để tiêu diệt con quái vật đâu.");
                        Console.ReadKey();
                        speak = false;
                    }
                    else
                    {
                        Console.WriteLine("Ôi! Vị hiệp sĩ thánh của chúng tôi, chúng tôi chờ bạn đã lâu, xin hãy nhận chiếc chìa khóa tiêu diệt con rồng tà ác.");
                        Console.WriteLine("__________________________");
                        Console.WriteLine("[01] Nhận chìa khóa");
                        Console.WriteLine("[00] Rời khỏi");
                        Console.WriteLine("_____________________");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                Console.Clear();
                                key = true;
                                Console.WriteLine("Bạn nhận được chìa khóa hầm ngục");
                                Console.ReadKey();
                                speak = false;
                                break;
                            case "0":
                                speak = false;
                                break;
                            default:
                                break;
                        }
                    }

                }
            }
            
            void CuaHang()
            {
                while (shop)
                {
                    Console.Clear();
                    Console.WriteLine("Ồ! Chào cậu trai trẻ!");
                    Console.WriteLine("Cậu tới đây có chuyện gì?");
                    Console.WriteLine("_____________________");
                    Console.WriteLine($"[01] Mua bình máu (10 vàng)| số bình máu hiện có {potion}");
                    Console.WriteLine($"[02] Nâng cấp tấn công (25 vàng)| Atk {atk}");
                    Console.WriteLine("[00] Rời khỏi");
                    Console.WriteLine("_____________________");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            if (gold >= 10)
                            {
                                Console.Clear();
                                potion += 1;
                                gold -= 10;
                                Console.WriteLine($"Mua thành công 1 bình máu | - 10 vàng");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Số vàng không đủ, vàng hiện tại {gold}");
                                Console.ReadKey();
                            }
                            break;
                        case "2":
                            if (gold >= 25)
                            {
                                Console.Clear();
                                atk += 5;
                                gold -= 25;
                                Console.WriteLine($"Tăng thành công 5 atk | - 25 vàng");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Số vàng không đủ, vàng hiện tại {gold}");
                                Console.ReadKey();
                            }
                            break;
                        case "0":
                            shop = false;
                            break;
                        default:
                            break;
                    }
                }
            }

            void Battle()
            {
                Console.Clear();
                standing = true;

                int index = rdn.Next(soquaivat.Length);
                string quaivatkey = soquaivat[index];
                quaivat.TryGetValue(quaivatkey, out Enemy enemy);

                while (battle)
                {
                    Console.Clear();
                    Console.WriteLine("____________________");
                    Console.WriteLine($"         {enemy.Name}");
                    Console.WriteLine($"Máu: {enemy.HpMax}/{enemy.Hp}");
                    Console.WriteLine($"Tấn công: {enemy.Attack}");
                    Console.WriteLine("____________________");
                    Console.WriteLine($"         {name}");
                    Console.WriteLine($"Máu: {hpMax}/{hp}");
                    Console.WriteLine($"Mana: {mpMax}/{mp}");
                    Console.WriteLine($"Tấn công: {atk}");
                    Console.WriteLine($"Bình máu: {potion}");
                    Console.WriteLine("____________________");

                    Console.WriteLine("[01] Tấn công");
                    if (potion > 0)
                    {
                        Console.WriteLine("[02] Sử dụng bình máu");
                    }
                    Console.WriteLine("[03] Xin giảng hòa :))");

                    Console.Write("> "); string choice = Console.ReadLine();
                    switch (choice) 
                    {
                        case "1":
                            Console.Clear();
                            enemy.Hp -= atk;
                            Console.WriteLine($"Bạn đã gây {atk} sát thương cho {enemy.Name}!");
                            if (hp > 0)
                            {
                                hp -= enemy.Attack;
                                Console.WriteLine($"{enemy.Name} đã tấn công {name} gây {enemy.Attack} sát thương");
                            }
                            Console.Write("> "); Console.ReadKey();
                            break;
                        case "2":
                            Console.Clear();
                            if (potion > 0)
                            {
                                potion -= 1;
                                Heal(30);
                                if (hp >= hpMax)
                                {
                                    hp = hpMax;
                                }
                                if (hp > 0)
                                {
                                    hp -= enemy.Attack;
                                    Console.WriteLine($"{enemy.Name} đã tấn công {name} gây {enemy.Attack} sát thương");
                                }
                            } else
                            {
                                Console.WriteLine("bạn không có bình hồi máu!");
                            }
                            Console.Write("> "); Console.ReadKey();
                            break;
                        case "3":
                            Console.Clear();
                            if (rdn.Next(0, 100) < 50)
                            {
                                printText.InVanBan("Xin [green]giảng hòa[/] thành công!\n", 15); Console.ReadKey();
                                printText.InVanBan("Con quái vật quyết định dắt bạn vào [red]hang[/] của nó!\n", 15); Console.ReadKey();
                                printText.InVanBan("...\n", 100);
                                Thread.Sleep(300);
                                printText.InVanBan("Sau [green]3[/] tháng ...\n", 15); Console.ReadKey();
                                printText.InVanBan("Bạn cuối cùng cũng được thả [green]tự do[/]\n", 15); Console.ReadKey();
                                printText.InVanBan("[green]Mở khóa thành tựu:[/] [yellow]Giảng hòa với quái vật[/]\n", 15); Console.ReadKey();
                                enemy.Hp = enemy.HpMax;
                                battle = false;
                            }
                            else
                            {
                                Console.WriteLine("Con quái vật không bị saygex, giảng hòa thất bại!");
                            }
                            break;
                        default:
                            Console.Clear();
                            break;
                    }

                    if (hp <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Chiến đấu thất bại, bạn đã thua quái vật {enemy.Name}");
                        battle = false;
                        play = false;
                        run = false;
                        hp = 0;
                        enemy.Hp = enemy.HpMax;
                        Console.WriteLine("GAME OVER");
                        Console.Write("> "); Console.ReadKey();
                    }
                    if (enemy.Hp <= 0)
                    {
                        Console.Clear();
                        printText.InVanBan($"Bạn đã [green]chiến thắng[/] {enemy.Name}\n", delayPerCharMs);
                        gold += enemy.dropGold;
                        printText.InVanBan($"Bạn đã nhận được [yellow]{enemy.dropGold}[/] Vàng\n", delayPerCharMs);
                        if (rdn.Next(0, 101) < 30)
                        {
                            printText.InVanBan($"Bạn nhận [green]1[/] được bình máu, số bình máu hiện có {potion + 1}\n");
                            potion += 1;
                        }
                        Console.ReadKey();
                        enemy.Hp = enemy.HpMax;
                        battle = false;
                    }
                }
            }

            void BossBattle()
            {
                Console.Clear();
                standing = true;
                quaivat.TryGetValue("dragon", out Enemy enemy);

                while (battle)
                {
                    Console.Clear();
                    Console.WriteLine("____________________");
                    Console.WriteLine($"         {enemy.Name}");
                    Console.WriteLine($"Máu: {enemy.HpMax}/{enemy.Hp}");
                    Console.WriteLine($"Tấn công: {enemy.Attack}");
                    Console.WriteLine("____________________");
                    Console.WriteLine($"         {name}");
                    Console.WriteLine($"Máu: {hpMax}/{hp}");
                    Console.WriteLine($"Mana: {mpMax}/{mp}");
                    Console.WriteLine($"Tấn công: {atk}");
                    Console.WriteLine($"Bình máu: {potion}");
                    Console.WriteLine("____________________");

                    Console.WriteLine("[01] Tấn công");
                    if (potion > 0)
                    {
                        Console.WriteLine("[02] Sử dụng bình máu");
                    }
                    Console.WriteLine("[03] Xin giảng hòa :))");

                    Console.Write("> "); string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            enemy.Hp -= atk;
                            Console.WriteLine($"Bạn đã gây {atk} sát thương cho {enemy.Name}!");
                            if (hp > 0)
                            {
                                hp -= enemy.Attack;
                                Console.WriteLine($"{enemy.Name} đã tấn công {name} gây {enemy.Attack} sát thương");
                            }
                            Console.Write("> "); Console.ReadKey();
                            break;
                        case "2":
                            Console.Clear();
                            if (potion > 0)
                            {
                                potion -= 1;
                                Heal(30);
                                if (hp >= hpMax)
                                {
                                    hp = hpMax;
                                }
                                if (hp > 0)
                                {
                                    hp -= enemy.Attack;
                                    Console.WriteLine($"{enemy.Name} đã tấn công {name} gây {enemy.Attack} sát thương");
                                }
                            }
                            else
                            {
                                Console.WriteLine("bạn không có bình hồi máu!");
                            }
                            Console.Write("> "); Console.ReadKey();
                            break;
                        case "3":
                            Console.Clear();
                            if (rdn.Next(0, 100) < 50)
                            {
                                printText.InVanBan("Xin [green]giảng hòa[/] thành công!\n", 15); Console.ReadKey();
                                printText.InVanBan("Con quái vật quyết định dắt bạn vào [red]hang[/] của nó!\n", 15); Console.ReadKey();
                                printText.InVanBan("...\n", 100);
                                Thread.Sleep(300);
                                printText.InVanBan("Sau [green]3[/] tháng ...\n", 15); Console.ReadKey();
                                printText.InVanBan("Bạn cuối cùng cũng được thả [green]tự do[/]\n", 15); Console.ReadKey();
                                printText.InVanBan("[green]Mở khóa thành tựu:[/] [yellow]Giảng hòa với quái vật[/]\n", 15); Console.ReadKey();
                                enemy.Hp = enemy.HpMax;
                                battle = false;
                            }
                            else
                            {
                                Console.WriteLine("Con quái vật không bị saygex, giảng hòa thất bại!");
                            }
                            break;
                        default:
                            Console.Clear();
                            break;
                    }

                    if (hp <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Chiến đấu thất bại, bạn đã thua quái vật {enemy.Name}");
                        battle = false;
                        play = false;
                        run = false;
                        enemy.Hp = enemy.HpMax;
                        Console.WriteLine("GAME OVER");
                        Console.Write("> "); Console.ReadKey();
                    }
                    if (enemy.Hp <= 0)
                    {
                        Console.Clear();
                        printText.InVanBan($"Bạn đã [green]chiến thắng[/] {enemy.Name} \n", delayPerCharMs);
                        gold += enemy.dropGold;
                        printText.InVanBan($"Bạn đã nhận được [yellow]{enemy.dropGold}[/] Vàng\n", delayPerCharMs);
                        if (rdn.Next(0, 101) < 30)
                        {
                            printText.InVanBan($"Bạn nhận [green]1[/] được bình máu, số bình máu hiện có {potion + 1}\n");
                            potion += 1;
                        }
                        Console.ReadKey();
                        enemy.Hp = enemy.HpMax;
                        battle = false;
                    }
                }
            }

            UpdateStats(); //Cập như chỉ số nếu loadgame, còn không thì chỉ số nhân vật vẫn vậy

            while (run)
            {
                while (menu)
                {
                    AnsiConsole.MarkupLine("ASCII RPG");
                    AnsiConsole.MarkupLine("[green][[01]][/] Trò chơi mới");
                    AnsiConsole.MarkupLine("[green][link=https://spectreconsole.net][[02]][/][/] Tiếp tục");
                    AnsiConsole.MarkupLine("[green][[03]][/] Luật lệ");
                    AnsiConsole.MarkupLine("[red][[00]][/] Thoát");

                    Console.Write("> ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            Console.Clear();
                            run = false;
                            menu = false;
                            break;
                        case "1":
                            Console.Clear();
                            menu = false;
                            play = true;
                            hp = 100;
                            hpMax = 100;
                            mp = 20;
                            mpMax = 20;
                            atk = 5;

                            potion = 0;
                            elixer = 0;
                            gold = 0;
                            x = 0;
                            y = 0;
                            UpdateStats();
                            printText.InVanBan("Chào mừng dũng sĩ đến với hành trình RPG\n", delayPerCharMs);
                            printText.InVanBan("Vui lòng nhập tên của bạn: ", delayPerCharMs);
                            name = Console.ReadLine();
                            Console.Clear();
                            break;
                        case "2":
                            Console.Clear();
                            stats.Clear();
                            Functions.LoadGame(path, ref stats, ref name);
                            LoadStatsFromList();
                            menu = false;
                            play = true;
                            break;
                        case "3":
                            Console.Clear();
                            menu = false;
                            rules = true;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Đầu vào không hợp lệ");
                            break;
                    }
                }
                while (play)
                {
                    Console.Clear();
                    string locationKey = map[y, x];
                    biom.TryGetValue(locationKey, out Locations location);

                    if (standing == false) 
                    {
                        if (location.CoQuaiVat)
                        {
                            if (rdn.Next(0, 101) < 30)
                            {
                                battle = true;
                                Battle();
                            }
                        }
                    }

                    Console.Clear();
                    Console.WriteLine("-------------------");

                    Console.WriteLine($"Nhân vật: {name}");
                    printText.InVanBan($"[red]HP[/]: {hpMax}/{hp} \n", delayPerCharMs);
                    printText.InVanBan($"[blue]MP[/]: {mpMax}/{mp} \n", delayPerCharMs);
                    printText.InVanBan($"[yellow]Atk[/]: {atk} \n", delayPerCharMs);
                    printText.InVanBan($"[yellow]Bình máu[/]: {potion} \n", delayPerCharMs);
                    printText.InVanBan($"[yellow]Vàng[/]: {gold} \n", delayPerCharMs);

                    Console.WriteLine("-------------------");

                    printText.InVanBan($"[yellow]Tọa Độ ({x}, {y})[/]: {location.Name} \n", delayPerCharMs);

                    Console.WriteLine("-------------------");

                    printText.InVanBan("[green]\\[01\\][/green]: Xem bản đồ \n", delayPerCharMs);
                    printText.InVanBan("[green]\\[02\\][/green]: Lưu game \n", delayPerCharMs);
                    
                    if (y > 0)
                    {
                        printText.InVanBan("[green]\\[03\\][/green]: Lên trên \n", delayPerCharMs);
                    }
                    if (y < y_len)
                    {
                        printText.InVanBan("[green]\\[04\\][/green]: Xuống dưới \n", delayPerCharMs);
                    }
                    if (x < x_len)
                    {
                        printText.InVanBan("[green]\\[05\\][/green]: Qua phải \n", delayPerCharMs);
                    }
                    if (x > 0)
                    {
                        printText.InVanBan("[green]\\[06\\][/green]: Qua trái \n", delayPerCharMs);
                    }
                    if (potion > 0)
                    {
                        printText.InVanBan("[green]\\[07\\][/green]: Sử dụng bình máu \n", delayPerCharMs);
                    }
                    if (map[y, x] == "cửa hàng")
                    {
                        printText.InVanBan("[green]\\[08\\][/green]: Vào cửa hàng \n", delayPerCharMs);
                    }
                    if (map[y, x] == "major")
                    {
                        printText.InVanBan("[green]\\[09\\][/green]: Gặp major \n", delayPerCharMs);
                    }
                    if (x == 6 && y == 4)
                    {
                        printText.InVanBan("[green]\\[10\\][/green]: Vào hang động \n", delayPerCharMs);
                    }

                    printText.InVanBan("[green]\\[11\\][/green]: Cài đặt \n", delayPerCharMs);
                    

                    printText.InVanBan("[green]\\[00\\][/green]: Trở lại \n", delayPerCharMs);
                    Console.Write("> ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            Console.Clear();
                            menu = true;
                            play = false;
                            break;
                        case "1":
                            Console.Clear();
                            for (int i = 0; i < map.GetLength(0); i++)
                            {
                                for (int j = 0; j < map.GetLength(1); j++)
                                {
                                    Console.Write($" {map[i, j]} ");
                                }
                                Console.WriteLine();
                            }
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case "2":
                            Console.Clear();
                            UpdateStats();
                            Functions.SaveGame(path, stats, name);
                            break;
                        case "3":
                            Console.Clear();
                            if (y > 0)
                            {
                                y -= 1;
                            }
                            standing = false;
                            break;
                        case "4":
                            Console.Clear();
                            if (y < y_len)
                            {
                                y += 1;
                            }
                            standing = false;
                            break;
                        case "5":
                            Console.Clear();
                            if (x < x_len)
                            {
                                x += 1;
                            }
                            standing = false;
                            break;
                        case "6":
                            Console.Clear();
                            if (x > 0)
                            {
                                x -= 1;
                            }
                            standing = false;
                            break;
                        case "7":
                            if (potion > 0)
                            {
                                potion -= 1;
                                Heal(30);
                                if (hp >= hpMax) { hp = hpMax; }
                            }
                            else
                            {
                                Console.WriteLine("bạn không có bình hồi máu!");
                            }
                            Console.Write("> "); Console.ReadKey();
                            break;
                        case "8":
                            if (map[y, x] == "cửa hàng")
                            {
                                shop = true;
                                CuaHang();
                            }
                            break;
                        case "9":
                            if (map[y, x] == "major")
                            {
                                speak = true;
                                Major();
                            }
                            break;
                        case "10":
                            if (x == 6 && y == 4)
                            {
                                if (key)
                                {
                                    battle = true;
                                    boss = true;
                                    BossBattle();
                                }else
                                {
                                    Console.WriteLine("Bạn không có chìa khóa!");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case "11":
                            play = false;
                            options = true;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Đầu vào không hợp lệ");
                            standing = true;
                            break;
                    }
                }
                while (rules)
                {
                    Console.WriteLine("Đang cập nhập");
                    Console.WriteLine("[00] Trở lại");

                    Console.Write("> ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            menu = true;
                            play = false;
                            break;
                        default:
                            Console.WriteLine("Đầu vào không hợp lệ");
                            break;
                    }
                }
                while (options)
                {
                    Console.Clear();
                    Console.WriteLine("Đang cập nhập");
                    Console.WriteLine("[01] Chữ nhanh");
                    Console.WriteLine("[00] Trở lại");

                    Console.Write("> ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            options = false;
                            play = true;
                            break;
                        case "1":
                            Console.Clear();
                            options = false;
                            play = true;
                            Console.WriteLine("Đã chỉnh tốc độ  chữ bằng 0");
                            delayPerCharMs = 0;
                            UpdateStats();
                            Functions.SaveGame(path, stats, name);
                            break;
                        default:
                            Console.WriteLine("Đầu vào không hợp lệ");
                            break;
                    }
                }

            }
        }
    }
}
