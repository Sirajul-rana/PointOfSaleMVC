using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PointOfSaleMVC.Manager;
using PointOfSaleMVC.Models;

namespace PointOfSaleMVC.Controllers
{
    public class ReportController : Controller
    {
        SetupManager setupManager = new SetupManager();
        ReportManager reportManager = new ReportManager();
        // GET: Report
        /*
         *  Sales report code starts here 
         */
        [HttpGet]
        public ActionResult SalesReport()
        {
            return View();
        }
        /*
         *  Sales report code ends here 
         */

        /*
         *  Purchase report code starts here 
         */
        [HttpGet]
        public ActionResult PurchaseReport()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }

        /*
         *  Purchase report code ends here 
         */

        /*
         *  Expense report code starts here 
         */
        [HttpGet]
        public ActionResult ExpenseReport()
        {
            return View();
        }

        /*
         *  Expense report code ends here 
         */

        /*
         *  Stock report code starts here 
         */
        [HttpGet]
        public ActionResult StockReport()
        {
            ViewBag.DropdownBranches = new SelectList(setupManager.GetBranches(), "BranchId", "BranchName", 0);
            return View();
        }

        [HttpPost]
        [ActionName("StockReport")]
        public ActionResult PrintStockReport(StockIn stockIn)
        {
            Branch branch = setupManager.GetBranch(stockIn.BranchId);
            List<StockIn> stockIns = reportManager.GetStocksByBranchId(stockIn);
            int sl = 1;
            Document pdfDoc = new Document(PageSize.A4, 50, 50, 50, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk(branch.BranchName, FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            //0=Left, 1=Centre, 2=Right
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 50f;//30

            //Cell no 2
            chunk = new Chunk(branch.BranchAddress, FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK));
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);
            chunk = new Chunk(branch.BranchContactNo, FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            //Add table to document
            pdfDoc.Add(table);
            

            chunk = new Chunk("Stock Report", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 1)));
            pdfDoc.Add(line);
            //Table
            table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell
            cell = new PdfPCell();
            chunk = new Chunk("SL");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(44, 62, 80);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Item Name");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(44, 62, 80);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Category Full Path");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(44, 62, 80);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Stock Quantity");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(44, 62, 80);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Price");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(44, 62, 80);
            table.AddCell(cell);

            foreach (StockIn aStockIn in stockIns)
            {
                table.AddCell((sl++).ToString());
                table.AddCell(aStockIn.Item.ItemName);
                table.AddCell(aStockIn.Item.Category.RootCategory.CategoryName + ">" + aStockIn.Item.Category.CategoryName + ">" + aStockIn.Item.ItemName);
                table.AddCell(aStockIn.Item.Quantity.ToString());
                table.AddCell((aStockIn.Item.SalePrice).ToString());
            }

            pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StockReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            return View();
        }

        [HttpPost]
        public ActionResult GetStocksByBranchId(StockIn stockIn)
        {
            List<StockIn> stockIns = reportManager.GetStocksByBranchId(stockIn);
            return Json(stockIns, JsonRequestBehavior.AllowGet);
        }
        /*
         *  Stock report code ends here 
         */

        /*
         *  Income report code starts here 
         */
        [HttpGet]
        public ActionResult IncomeReport()
        {
            return View();
        }
        /*
         *  Income report code ends here 
         */
    }
}