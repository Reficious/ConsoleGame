# Hướng dẫn sử dụng và tùy chỉnh RPG ASCII CSHARP made by Thạch Sùng dev

## Hiểu:
(*) Trước tiên thì hãy nhìn sơ qua về tất cả file có trong Folder game này.

- Enemy.cs
- Functions.cs
- Location.cs
- printText.cs
- start_game.cs

- Tổng quan, thì tôi chỉ có file start_game.cs chứa tất cả mã để chạy game, 
còn các file C# khác thì tôi dùng để chứa các chức năng được dùng trong start_game.cs.

- Enemy.cs (chứa các thuộc tính của quái vật)
	+ class Enemy : chứa 1 lớp các dữ liệu của quái vật như máu, tấn công, ...

- Functions.cs (chứa các phương thức dùng trong game, hiện tại có save, load game.)
	+ SaveGame() : Chức năng lưu game.
	+ LoadGame() : Chức năng load lại dữ liệu đã lưu.

- Locations.cs (chứa các thuộc tính của 1 vị trí trong map)
	+ class Locations : chứa dữ liệu (tên, có quái vật spawn ra)

- prinText.cx (chứa các phương thức (hay hàm bên python) dùng để in ra các văn bản ra màn hình)
	+ InVanBan() : Dùng in văn bản.

(*) start_game.cs (File chính của game)
- Khai báo : chứa các dữ liệu cần thiết của game.
- Phương thức : một số phương thức, dùng việc sử dụng dễ hơn.
- Chạy màn hình : phần in ra những gì người chơi thấy.

## Cách thức hoạt động

### Biến trong start_game.cs
- Trong start_game.cs có các biến bool, dùng để khai báo trạng thái mà người chơi lựa chọn, những thứ mà code nên chạy.

- Phần biến chính, hiện ra màn hình:
	+ run
	+ menu
	+ play 
	+ rules
	+ options

- Phần biến phụ, để kiểm tra và sử dụng:
	+ battle : dùng hiện màn hình chiến đấu.
	+ standing : dùng để kiểm tra người chơi có đang đứng yên ko, vì khi vô game.
bạn không muốn vừa mới vô và đang đứng trên chỗ spawn quái, cái phải vào trận chiến hoặc,
đang sử dụng bình máu là gặp quái vật.
	+ speak : dùng để nói chuyện với major
	+ key : dùng để mở màn đấu boss
	+ shop : dùng để mở màn hình cửa hàng
	+ boss : kiểm tra xem bạn đang có trong phòng boss và vào đấu boss.

- Cụ thể, sử dùng vòng lặp while
 run 
 {
	menu, 
	play 
	{
		options
	}, 
	rules
}
B1: Kiểm tra biến `run` có `true` ko, nếu `true` là đang chơi game.
B2: Kiểm tra biến `menu`, `play`, `rules` cái nào `true`, thì hiện cái đó.
B3: Chạy màn hình của các biến true
- Menu : thì có các lựa chọn như chơi mới, chơi lại, xem luật, thoát
- Play : Hiện màn hình game (thông tin của người chơi, tọa độ, ..), hiện ra các thao tác của người chơi như qua trái, qua phải, mở cửa hàng, ...
- Rules : Hiện ra luật lệ game
- Options : nằm trong màn hình của Play, dùng để thay đổi về trò chơi.

- Phần biến người chơi: Lưu các giá trị khởi đầu của một người chơi mới
	+ name : tên
	+ hp, hpMax : máu và máu tối đa (dùng để set lại máu sau khi chết)
	+ mp, mpMax : cx tương tự như trên.
	+ atk : tấn công của người chơi.

	+ potion : hiện số bình máu của người chơi
	+ elixer : hiện số thuốc tiên của người chơi (tôi có tạo ra mà không sử dụng)
	
	+ gold : số vàng của người chơi.
	+ x, y : tọa độ của người chơi, dùng để đối chiều vào mảng 2 chiều map[y][x].
	
	+ delayPerCharMs : thời gian để các chữ hiện ra, đơn vị mili giây.

- Phần biến của game: Lưu các giá trị cơ bản của game
	+ map[,] : mảng 2 chiều để tạo ra 1 bảng giá trị lưu các địa điểm trên bản đồ.
trong mã chính, tôi có note thêm chiều x, y nhưng thực tế phải là ngược lại, nên lúc dùng map[y][x], mới hiện đúng vị trí trong game, đặt x, y cho dễ nhìn thôi,

	+ biom : đây là từ điển của các địa điểm trong map,
cụ thể thì nó định nghĩa vị trí đó, địa điểm đó tiêu đề là gì và nơi đó có spawn quái vât ko/

	+ x_len, y_len : phần biến dùng để xác đinh giới hạn của tọa độ, tránh gây lỗi quá dữ liệu được tạo trong mảng.

	+ soquaivat : cũng như map, nó lưu các con quái vật trong game,
Lưu ý, những quái vật lưu trong đây thì nó sẽ xuất hiện trong random chọn quái

	+ quaivat : như biom, là từ điển định nghĩa quái vật sẽ có những gì.

- Khai báo một số thứ quan trọng:
	+ stats : đây là một danh sách, không phải mảng nó dùng để lưu các chỉ số của người chơi.
	+ rdn : đây là đối tượng, dùng để random số, random quái vật.

### Hàm trong start_game.cs:
- UpdateStats : đây là phương thức cập nhập những gì bạn đã làm trong game,
giả dụ như vừa tăng 10 cấp, tăng 10 atk, ... và chúng ta cần phải lưu các giá trị này,
vào biến `stats` -> từ đó thì phương thức SaveGame() mới lưu lại những gì đã làm.

- LoadStatsFromList : đây là phương thức dùng để nhập các dữ liệu từ file savegame vào các,
chỉ số trong game, thì game mới thay đổi những giá trị mặc định khi tạo nhân vật mới.

- Heal : gọi tới nó để sử dụng bình máu
- Major : gọi tới nó để nói chuyện với nhân vật (dùng phương thức, thay vì các biến vì nó tiện và không có dùng nhiều)
- CuaHang : gọi tới để mở cửa hàng, lý do tương tự trên.
- Battle : gọi tới màn hình chiến đấu
- BossBattle : như trên, nhưng dùng để xử lý vấn đề thay vì random quái, mà đấu với boss.

### Còn lại, là sử dụng các chức năng ở các file khác đã làm sẵn, hiện màn hình, đưa ra lựa chọn cho người chơi sử dụng.

### Những thứ có thể làm:
1. Bro có thể biến những thứ lặp đi lặp lại thành 1 phương thức, và gọi nó 1 lần là được,
giúp tránh việc 1 đoạn mã lặp đi lặp lại nhiều lần.

2. Bro có thể tạo thêm nhiều map và cụ thể hơn, ví dụ map làng, hang động, rừng có những gì
nơi nào không thể di chuyển, ...

3. Mở thêm các tính năng khác như inventory, items, quest, ...

4. Đơn giản hóa lại, hiện tại tôi chỉ code cho có nên dồn vào 1 file, bro có thể viết các chức năng sau đó gọi tới

5. Tách ra các file riêng biệt, xử lý mọi thứ thuật tiện hơn, ví dụ
bro chỉ dùng file chính để lưu các biến thôi, chỉ số cơ bản, màn hình game, map, quái vật, ...
sau đó bro gọi tới các màn hình, các thứ đã làm sẵn từ file chính thôi

ví dụ:

run 
{
	menu 
	{
		goiMenu(); : hiện các menu game, vào play, ...
	}

	play 
	{
		goiBangChiSo() : hiện các chức năng
		goiChucNang() : hiện các điều khiển game
			{ trong chức năng
				Battle : hiện màn hình chiến đấu
				Shop : hiện màn hình cửa hàng
			}
	}

	rules 
	{
		
	}
	
}