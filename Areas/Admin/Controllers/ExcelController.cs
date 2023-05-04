using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using OfficeOpenXml.Style;
using Microsoft.EntityFrameworkCore;
using WEB2.Models;
using System.Threading.Tasks;
using WEB2.Data;

namespace WEB2.Controllers
{
    
    public class ExcelController : Controller
    {
        private readonly AppDbContext _context;

        public ExcelController(AppDbContext context)
        {
          
            _context = context;
        }


        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> XuatFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = new DataTable();
            var stream = new MemoryStream();
            //int count;
           
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Danh sach các đơn bán hàng đã hoàn thành");
                sheet.Cells["A1:H99"].Style.Font.Name = "Times New Roman";
                sheet.Cells["A1:I1"].Merge = true;
               
                sheet.Column(3).Width = 25;
                sheet.Column(1).Width = 5.33;
                sheet.Column(2).Width = 11.67;
                sheet.Column(5).Width = 20;
                sheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A1:C1"].Value = "CỬA HÀNG BÁN ĐỒ CÔNG NGHỆ TECHN";

                sheet.Cells["D6:E6"].Merge = true;
                sheet.Cells["F6:G6"].Merge = true;
                sheet.Cells["H6:I6"].Merge = true;
                sheet.Row(9).Height = 41.13;
                sheet.Cells["A6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A6:I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells["A6:I6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells["A6:I6"].Style.Fill.BackgroundColor.SetColor(0, 186, 248, 255);
                sheet.Cells["A6:I6"].Style.Font.Bold = true;
                sheet.Cells["A6:I6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells["A6:I6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

               
                sheet.Cells["A6:A6"].Value = "STT";
                sheet.Cells["B6:B6"].Value = "Mã hóa đơn";
                sheet.Cells["C6:C6"].Value = "Tên khách hàng";
                sheet.Cells["D6:D6"].Value = "Tình trạng đơn hàng";
                sheet.Cells["F6:F6"].Value = "Số tiền";
                sheet.Cells["H6:H6"].Value = "Ngày đặt hàng";
                sheet.Name = "Danh sách đơn hàng";
                var query = await _context.Order.Include(p => p.Customer).Include(q => q.Customer.AppUser)
                    .Where(k => k.TransactStatus == "done").ToListAsync();
                int x = 7;
                int y = 7;
                int z = 1;
                foreach (var p in query)
                {
                    string a = "A"+ x+":A"+ y;
                    sheet.Cells[a].Value = z;
                    string b = "B" + x + ":B" + y;
                    sheet.Cells[b].Value = p.OrderId;
                    string c = "C" + x + ":C" + y;
                    sheet.Cells[c].Value = p.Customer.AppUser.FullName;

                    string e = "D" + x + ":E" + y;
                    sheet.Cells[e].Merge = true;

                    string d = "D" + x + ":D" + y;
                    sheet.Cells[d].Value = "Đã hoàn thành";


                    string g = "F" + x + ":G" + y;
                    sheet.Cells[g].Merge = true;

                    string f = "F" + x + ":F" + y;
                    sheet.Cells[f].Value = p.Paid;


                    
                    string i = "H" + x + ":I" + y;
                    sheet.Cells[i].Merge = true;
                   
                    string h = "H" + x + ":H" + y;
                    sheet.Cells[h].Style.Numberformat.Format = "dd/MM/yyyy";
                    sheet.Cells[h].Value = p.OrderDay;
                    x = x + 1;
                    y = y + 1;
                    z = z + 1;
                }

                package.Save();


            }
            stream.Position = 0;

            var tenfile = $"DS_BanHangHoanThanh_{DateTime.Now}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", tenfile);
        }
    }
}
