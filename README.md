# Nhóm 22
## 
Phát triển ứng dụng web - IS207.N12

# Tên đồ án: Xây dựng website bán laptop, điện thoại,...
  website sẽ thực hiện việc mua và bán các sản phẩm công nghệ như laptop, điện thoại,...Ở mỗi sản phẩm có thể xem các cấu hình chi tiết và các thông tin liên quan đến sản phẩm
  và nếu người dùng đăng nhập thì có thể để lại đánh giá. Ở trang Admin có chức năng thống kê để kiểm tra số liệu và các hoạt động của website.
 
## Thành viên nhóm

| Tên thành viên | MSSV | Facebook | SDT | Nhiệm vụ | Đánh giá |
| ------ | ------ | ------ | ------ | ------ | ------ |
| Nguyễn Hữu Thắng | 20520759 | https://www.facebook.com/BOT0211 | 08 6815 0937 | Xây dựng web | 100% |

  
# Chức năng

#### Chung

- Đăng nhập/ đăng ký/ đăng xuất/ logout / nhớ mật khẩu

- Phân quyền (admin, staff, customer)

- Lấy lại password bằng gmail

- Chi tiết sản phẩm (được lọc theo phiên bản và màu sắc của sản phẩm)

- GỢi ý các sản phẩm tương tự (Comming soon....)

- Gửi mail để xác nhận đăng nhập và reset mật khẩu (chức năng đang phát triển)

#### Thành viên

- Giỏ hàng

- Thanh toán (tiền mặt/ VNpay) (chưa có ship và khuyến mãi)

- Quản lý thông tin cá nhân (đóng tài khoản, cập nhật mật khẩu, xác thực 2 lớp(chức năng đang phát triển), backup dữ liệu cá nhân, liên kết tài khoản với google và facebook)

- Đơn hàng đã mua, theo dõi tình trạng đơn hàng.


     
#### Admin


- Tổng quan bán hàng, thống kê thu chi

- Quản lý bán hàng 


## Tech

- [C#] 
- [.Net 6.0] 
- [ASP.Net Core MVC]
- [Bootstrap] 
- [Identity]
- [Thư viện kèm theo]

## Hướng dẫn cài đặt    

- website sử dụng: 
> IDE: [Visual Studio 2022]
Database: [SQLServer]

File database trong thư mục Data-> Database_Design

Dữ liệu được lưu dưới dạng .sql trong thư mục Data -> Script 

- B1: clone web từ github: bật terminal, powercell, cmnd.... và chạy câu lệnh sau

```sh
    
```


- B2: Chạy file backup_database.sql trong data -> Script trên sqlserver

- B3: chỉnh lại đường dẫn ở appsettings.js: 
  đổi serve là Server của người dùng, và database là tên database đã tạo trong file .sql ở trên 


``` 
 "ConnectionStrings": {
    "DefaultConnection": "Server = <tên server>; Database = <tên database>  ; Trusted_Connection=True;MultipleActiveResultSets=True"
  }
```
- B4: Chạy project bằng cách ấn ctrl + f5 trong [Visual Studio 2022]

**Readme by [Thang Huu]**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

 [C#]: <https://docs.microsoft.com/vi-vn/dotnet/csharp/>
 [.Net 6.0]: <https://dotnet.microsoft.com/en-us/download/dotnet/6.0>
 [ASP.Net Core MVC]: <https://docs.microsoft.com/vi-vn/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-2.1&tabs=visual-studio>
 [Bootstrap]: <https://getbootstrap.com/>
 [Visual Studio 2022]: <https://visualstudio.microsoft.com/downloads/>
 [SQLServer]: <https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
 [SignalR]: <https://dotnet.microsoft.com/en-us/apps/aspnet/signalr>
 [Identity]: <https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio>