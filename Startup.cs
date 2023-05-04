using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Text.Json.Serialization;
using WEB2.Areas.Order;
using WEB2.Data;
using WEB2.Hubs;
using WEB2.Infrastructure.Respository;
using WEB2.Mail;
using WEB2.Models;

namespace WEB2 {

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
            ));
            // services.AddSignalR(); services.AddTransient<IChatRepository, ChatRepository>();

            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "ThangHuu";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session: 30p
            });

            // Đăng ký AppDbContext, sử dụng kết nối đến MS SQL Server
            services.AddDbContext<AppDbContext>(options => {
                string connectstring = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectstring);
            });
            // Đăng ký các dịch vụ của Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false; // Email khong là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại
            });

            // Cấu hình Cookie
            services.ConfigureApplicationCookie(options => {
                // options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = $"/login/"; // Url đến trang đăng nhập
                options.LogoutPath = $"/logout/";
                options.AccessDeniedPath = $"/AccessDenied/"; // Trang khi User bị cấm truy cập
            });
            services.Configure<SecurityStampValidatorOptions>(options => {
                // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role) SecurityStamp trong
                // bảng User đổi -> nạp lại thông tinn Security
                options.ValidationInterval = TimeSpan.FromSeconds(3);
            });

            services.Configure<RouteOptions>(options => {
                options.AppendTrailingSlash = false; // Thêm dấu / vào cuối URL
                options.LowercaseUrls = true; // url chữ thường
                options.LowercaseQueryStrings = false; // không bắt query trong url phải in thường
            });

            services.AddOptions(); // Kích hoạt Options
                                   // Add our Config object so it can be injected
            services.Configure<MyConfig>(Configuration.GetSection("MyConfig"));
            var mailsettings = Configuration.GetSection("MailSettings"); // đọc config
            services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject

            services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
            services.AddAuthorization(options => {
                // User thỏa mãn policy với role admin
                options.AddPolicy("AdminDropdown", policy => {
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("StaffDropdown", policy => {
                    policy.RequireRole("Nhân viên", "Admin");
                });
                options.AddPolicy("ManangerDropdown", policy => {
                    policy.RequireRole("Quản Lý");
                });

                options.AddPolicy("Admin", policy => {
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("Manager", policy => {
                    policy.RequireRole("Admin", "Quản Lý");
                });
                options.AddPolicy("Staff", policy => {
                    policy.RequireRole("Nhân viên", "Admin", "Quản Lý");
                });
            });
            /* The relevant part for Forwarded Headers */
            services.Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All;
            });

            services.AddAuthentication()
            .AddGoogle(googleOptions => { // Đọc thông tin Authentication:Google từ appsettings.json
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");

                // Thiết lập ClientID và ClientSecret để truy cập API google
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                googleOptions.CallbackPath = "/dang-nhap-tu-google";
            })
             .AddFacebook(facebookOptions => {
                 // Đọc cấu hình
                 IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                 facebookOptions.AppId = facebookAuthNSection["AppId"];
                 facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
                 // Thiết lập đường dẫn Facebook chuyển hướng đến
                 facebookOptions.CallbackPath = "/dang-nhap-tu-facebook";
             });
            // services.AddCors();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            Console.WriteLine(env.WebRootPath);
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseForwardedHeaders();

            app.UseStaticFiles();

            app.UseSession();         // Đăng ký Middleware Session vào Pipeline

            app.UseRouting();

            app.UseCors("ApiCorsPolicy");

            app.UseHttpsRedirection();
            app.UseAuthentication(); // Phục hồi thông tin đăng nhập (xác thực)
            app.UseAuthorization(); // Phục hồi thông tinn về quyền của User

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Đến Razor Page
                endpoints.MapRazorPages();

                //endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.Map("/testapi", app => {
                app.Run(async context => {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var ob = new {
                        url = context.Request.GetDisplayUrl(),
                        content = "Trả về từ testapi"
                    };
                    string jsonString = JsonConvert.SerializeObject(ob);
                    await context.Response.WriteAsync(jsonString, Encoding.UTF8);
                });
            });

            app.Run(async (HttpContext context) => {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Page not found!");
            });
        }
    }
}