using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using LevelStore.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LevelStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private IProductRepository repositoryProduct;
        private Cart cart;
        private readonly IHostingEnvironment _appEnvironment;

        public OrderController(IOrderRepository repoService, Cart cartService, IHostingEnvironment appEnvironment, IProductRepository repoProduct)
        {
            repositoryProduct = repoProduct;
            repository = repoService;
            cart = cartService;
            _appEnvironment = appEnvironment;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
        
        public ViewResult ListOrder()
        {
            TempData["BindedColors"] = repositoryProduct.TypeColors.ToList();
            return View(repository.Orders);
        }

        
        public IActionResult ChangeStatus(int status, int orderId)
        {
            if (Enum.IsDefined(typeof(OrderStatus), status))
            {
                repository.ChangeStatus((OrderStatus)status, orderId);
            }
            return RedirectToAction(nameof(ListOrder));
        }

        public IActionResult ChangeOrder(int orderId)
        {
            Order order = repository.Orders.FirstOrDefault(i => i.OrderID == orderId);
            TempData["JsonOrder"] = JsonConvert.SerializeObject(order);
            if (order == null)
            {
                return RedirectToAction("ListOrder");
            }
            TempData["BindedColors"] = repositoryProduct.TypeColors.ToList();
            return View("ViewSingleOrder", order);
        }

        [HttpPost]
        public ViewResult ChangeOrder(Order order)
        {
            return View("ListOrder");
        }

        [HttpPost]
        public IActionResult ChangeOrderAjax([FromBody] Order order)
        {
            if (order != null && Enum.IsDefined(typeof(OrderStatus), order.Status))
            {
                repository.ChangeOrder(order);
                return Json("Succes");
            }
            return Json("Fail");
        }

        public IActionResult DownloadExcel(int[] checkedOrderId)
        {
            List<Order> orderList = repository.Orders.Where(oi => checkedOrderId.Any(coi => coi.Equals(oi.OrderID)))
                .ToList();
            List<TypeColor> colors = repositoryProduct.TypeColors.ToList();
            string sWebRootFolder = _appEnvironment.WebRootPath + "/excelReports";
            string sFileName = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                .Replace("  ", "_").Trim()
                .Replace(" ", "_")
                .Replace('/', '_')
                .Replace(':', '_')
                + "_Report.xlsx";
            
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                XSSFWorkbook workbook = new XSSFWorkbook();
                
                IFont boldFont = workbook.CreateFont();
                boldFont.Boldweight = (short)FontBoldWeight.Bold;
                ICellStyle boldStyle = workbook.CreateCellStyle();
                boldStyle.SetFont(boldFont);

                ICell cell;
                ISheet excelSheet = workbook.CreateSheet("Заказы");
                excelSheet.DefaultColumnWidth = 20;
                IRow row = excelSheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("Имя");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue("Телефон");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue("Продукт");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(5);
                cell.SetCellValue("Статус");
                cell.CellStyle = boldStyle;

                
                int globalIndexRow = 0;
                for (int i = 1; i <= orderList.Count; i++)
                {

                    row = excelSheet.CreateRow(i + globalIndexRow);
                    row.CreateCell(0).SetCellValue(orderList[i-1].FirstName + " " + orderList[i - 1].LastName);
                    row.CreateCell(1).SetCellValue(orderList[i-1].Phone);

                    cell = row.CreateCell(2);
                    cell.SetCellValue("Название продукта");
                    cell.CellStyle = boldStyle;
                    cell = row.CreateCell(3);
                    cell.SetCellValue("Количество");
                    cell.CellStyle = boldStyle;
                    cell = row.CreateCell(4);
                    cell.SetCellValue("Цена");
                    cell.CellStyle = boldStyle;
                    
                    if (orderList[i - 1].Status == (int) OrderStatus.Waiting)
                    {
                        row.CreateCell(5).SetCellValue("Ожидание");
                    }
                    else if (orderList[i - 1].Status == (int) OrderStatus.NotSended)
                    {
                        row.CreateCell(5).SetCellValue("Не отослано");
                    }
                    else
                    {
                        row.CreateCell(5).SetCellValue("Отослано");
                    }
                    foreach (var line in orderList[i-1].Lines)
                    {
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        row.CreateCell(2).SetCellValue(line.Product.Name);
                        row.CreateCell(3).SetCellValue(line.Quantity);
                        row.CreateCell(4).SetCellValue(line.Product.Price.ToString("C"));
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        cell = row.CreateCell(2);
                        cell.SetCellValue("Фурнитура");
                        cell.CellStyle = boldStyle;
                        row.CreateCell(3).SetCellValue(line.Furniture == (int) Furniture.Nikel ? "Никель" : "Антик");
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        cell = row.CreateCell(2);
                        cell.SetCellValue("Цвет");
                        cell.CellStyle = boldStyle;
                        row.CreateCell(3).SetCellValue(colors.FirstOrDefault(cn => cn.TypeColorID == line.SelectedColor)?.ColorType ?? "Неизвестный");

                    }
                    globalIndexRow++;
                    row = excelSheet.CreateRow(i + globalIndexRow);
                    cell = row.CreateCell(2);
                    cell.SetCellValue("Общая цена");
                    cell.CellStyle = boldStyle;
                    row.CreateCell(3).SetCellValue(orderList[i-1].Lines.Sum(s => s.Quantity * s.Product.Price).ToString("C"));
                }
                workbook.Write(fs);
            }

            var filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/excelReports/{sFileName}");
            const string fileType = "application/xlsx";
            return PhysicalFile(filePath, fileType, sFileName);
        }
    }
}
