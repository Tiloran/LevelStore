using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LevelStore.Models;
using LevelStore.Models.Enums;
using LevelStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LevelStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _repositoryProduct;
        private readonly IShareRepository _repositoryShares;
        private readonly Cart _cart;
        private readonly IHostingEnvironment _appEnvironment;

        public OrderController(IOrderRepository repoService, Cart cartService, IHostingEnvironment appEnvironment, IProductRepository repoProduct, IShareRepository repoShare)
        {
            _repositoryProduct = repoProduct;
            _repository = repoService;
            _cart = cartService;
            _appEnvironment = appEnvironment;
            _repositoryShares = repoShare;
        }

        

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }
        
        public ViewResult ListOrder(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 3;
            int count = _repository.Orders.Count();
            var items = _repository.Orders.OrderByDescending(d => d.DateOfCreation).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            OrderListViewModel orderListviewModel = new OrderListViewModel
            {
                PageViewModel = pageViewModel,
                Orders = items
            };

            TempData["BindedColors"] = _repositoryProduct.TypeColors.ToList();
            //TempData["Shares"] = _repositoryShares.Shares.ToList();
            return View(orderListviewModel);
        }

        
        public IActionResult ChangeStatus(int status, int orderId)
        {
            if (Enum.IsDefined(typeof(OrderStatus), status))
            {
                _repository.ChangeStatus((OrderStatus)status, orderId);
            }
            return RedirectToAction(nameof(ListOrder));
        }

        public IActionResult ChangeOrder(int orderId)
        {
            Order order = _repository.Orders.FirstOrDefault(i => i.OrderID == orderId);
            TempData["JsonOrder"] = JsonConvert.SerializeObject(order);
            if (order == null)
            {
                return RedirectToAction("ListOrder");
            }
            TempData["BindedColors"] = _repositoryProduct.TypeColors.ToList();
            return View("ViewSingleOrder", order);
        }
        

        [HttpPost]
        public IActionResult ChangeOrderAjax([FromBody] Order order)
        {
            if (order != null && Enum.IsDefined(typeof(OrderStatus), order.Status))
            {
                _repository.ChangeOrder(order);
                return Json("Succes");
            }
            return Json("Fail");
        }

        public IActionResult DownloadExcel(int[] checkedOrderId)
        {
            List<Order> orderList = _repository.Orders.Where(oi => checkedOrderId.Any(coi => coi.Equals(oi.OrderID)))
                .ToList();
            List<TypeColor> colors = _repositoryProduct.TypeColors.ToList();
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
                cell.SetCellValue("Дата создания");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue("Имя");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue("Телефон");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(3);
                cell.SetCellValue("Продукт");
                cell.CellStyle = boldStyle;
                cell = row.CreateCell(6);
                cell.SetCellValue("Статус");
                cell.CellStyle = boldStyle;

                
                int globalIndexRow = 0;
                for (int i = 1; i <= orderList.Count; i++)
                {

                    row = excelSheet.CreateRow(i + globalIndexRow);
                    row.CreateCell(0).SetCellValue(orderList[i - 1].DateOfCreation.ToString());
                    row.CreateCell(1).SetCellValue(orderList[i-1].FirstName + " " + orderList[i - 1].LastName);
                    row.CreateCell(2).SetCellValue(orderList[i-1].Phone);

                    cell = row.CreateCell(3);
                    cell.SetCellValue("Название продукта");
                    cell.CellStyle = boldStyle;
                    cell = row.CreateCell(4);
                    cell.SetCellValue("Количество");
                    cell.CellStyle = boldStyle;
                    cell = row.CreateCell(5);
                    cell.SetCellValue("Цена");
                    cell.CellStyle = boldStyle;
                    
                    if (orderList[i - 1].Status == (int) OrderStatus.Waiting)
                    {
                        row.CreateCell(6).SetCellValue("Ожидание");
                    }
                    else if (orderList[i - 1].Status == (int) OrderStatus.NotSended)
                    {
                        row.CreateCell(6).SetCellValue("Не отослано");
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue("Отослано");
                    }
                    decimal totalPrice = 0;
                    foreach (var line in orderList[i-1].Lines)
                    {
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        row.CreateCell(3).SetCellValue(line.Product.Name);
                        row.CreateCell(4).SetCellValue(line.Quantity);
                        decimal price = line.PriceAfterCheckout;
                        if (line.KoefPriceAfterCheckout != null && line.FakeShare == false)
                        {
                            price = line.PriceAfterCheckout / 100 * (decimal)(100 - line.KoefPriceAfterCheckout);
                        }
                        row.CreateCell(5).SetCellValue(price.ToString("C"));
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        cell = row.CreateCell(3);
                        cell.SetCellValue("Фурнитура");
                        cell.CellStyle = boldStyle;
                        row.CreateCell(4).SetCellValue(line.Furniture == (int) Furniture.Nikel ? "Никель" : "Антик");
                        globalIndexRow++;
                        row = excelSheet.CreateRow(i + globalIndexRow);
                        cell = row.CreateCell(3);
                        cell.SetCellValue("Цвет");
                        cell.CellStyle = boldStyle;
                        row.CreateCell(4).SetCellValue(colors.FirstOrDefault(cn => cn.TypeColorID == line.SelectedColor)?.ColorType ?? "Неизвестный");
                        totalPrice += price * line.Quantity;
                    }
                    globalIndexRow++;
                    row = excelSheet.CreateRow(i + globalIndexRow);
                    cell = row.CreateCell(3);
                    cell.SetCellValue("Общая цена");
                    cell.CellStyle = boldStyle;
                    row.CreateCell(4).SetCellValue(totalPrice.ToString("C"));
                }
                workbook.Write(fs);
            }

            var filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/excelReports/{sFileName}");
            const string fileType = "application/xlsx";
            return PhysicalFile(filePath, fileType, sFileName);
        }
    }
}
